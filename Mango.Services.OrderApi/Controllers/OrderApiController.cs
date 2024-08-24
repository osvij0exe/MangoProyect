using AutoMapper;
using Mango.MessageBus.Service.Interface;
using Mango.Services.OrderApi.Data;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Models.Dtos;
using Mango.Services.OrderApi.Services.Interfaces;
using Mango.Services.OrderApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;

namespace Mango.Services.OrderApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderApiController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _response;

        public OrderApiController(AppDbContext context,
            IProductService productService,
            IMapper mapper,
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _context = context;
            _productService = productService;
            _mapper = mapper;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new ResponseDto();
        }


        [HttpGet("GetOrders")]
        [Authorize]
        public ResponseDto? Get(string userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;

                if(User.IsInRole(SD.RoleAdmin))
                {
                    objList = _context.OrderHeader.Include(u => u.OrderDetails).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _context.OrderHeader.Include(u => u.OrderDetails)
                        .Where(u => u.UserId == userId)
                        .OrderByDescending(u => u.OrderHeaderId).ToList();

                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);


            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetOrder/{id:int}")]
        [Authorize]
        public ResponseDto? Get(int id)
        {
            try
            {


                OrderHeader orderHeader = _context.OrderHeader.Include(u => u.OrderDetails).First(u => u.OrderHeaderId == id);
                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                _response.IsSucess = true;

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }



        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<ResponseDto> CreateOrder([FromBody]CartDto cartDto)
        {
            try
            {

                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);

                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _context.OrderHeader.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _context.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId  = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
                _response.IsSucess = true;


            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        // se requiere una cuenta de stripe
        [HttpPost("CreateStripeSession")]
        [Authorize]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {

            try
            {

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApproveUrl,
                    CancelUrl = stripeRequestDto.CanelUrl,
                    //item display on checkout page
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),// example $20.99 -> 2099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductDto.Name

                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _context.OrderHeader.First(u => u.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _context.SaveChanges();

                _response.Result = stripeRequestDto;
                _response.IsSucess = true;
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSucess = false;
            }

            return _response;
        }

        // se requiere una cuenta de stripe
        [HttpPost("ValidateStripeSession")]
        [Authorize]
        public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)
        {

            try
            {

                OrderHeader orderHeader = _context.OrderHeader.First(u => u.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);
                //paymentIntent ver documentacion stripe
                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status == "succeeded") //ver documentacion y video curso microservicios udemy clas 136
                {
                    //then payment was succesful
                    orderHeader.PaymentIntenrId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _context.SaveChanges();

                    RewardsDto rewardsDto = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                        UserId = orderHeader.UserId
                    };

                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                    await _messageBus.PublishMessage(rewardsDto, topicName);

                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                    _response.IsSucess = true;
                }


            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSucess = false;
            }

            return _response;
        }

        // se requiere suscripcion de stripe
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        [Authorize]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody]string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _context.OrderHeader.First(u => u.OrderHeaderId == orderId);
                if(orderHeader != null)
                {
                    if(newStatus == SD.Status_Cancelled)
                    {
                        //we wiil give refund
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntenrId,
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);
                        orderHeader.Status = newStatus;
                    }
                    orderHeader.Status = newStatus;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
            }
            return _response;
        }
    }
}
