using AutoMapper;
using Mango.MessageBus.Service.Interface;
using Mango.Services.ShoppingCartApi.DataAccess;
using Mango.Services.ShoppingCartApi.Model;
using Mango.Services.ShoppingCartApi.Model.Dto;
using Mango.Services.ShoppingCartApi.Service.Iservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCarApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductService _productServices;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ResponseDto _response;

        public ShoppingCarApiController(AppDbContext context,
            IMapper mapper,
            IProductService productServices,
            ICouponService couponService,
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _productServices = productServices;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new ResponseDto();
        }




        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _context.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);

                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _context.CartHeaders.Update(cartFromDb);
                await _context.SaveChangesAsync();
                _response.Result = true;
                _response.IsSucess = true;  

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }


        //Nota: Ser requiere una suscripcion de AZURE y crear un serviceBus 
        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {

                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")!);
                _response.Result = true;
                _response.IsSucess = true;

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }



        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {

                var cartHeaderFromDb = await _context.CartHeaders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

                if(cartHeaderFromDb == null)
                {
                    // create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    await _context.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;

                    _context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();

                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetaislFromDb = await _context.CartDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                        u=> u.ProductId == cartDto.CartDetails.First().ProductId && 
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if(cartDetaislFromDb == null)
                    {
                        // create cartdetals
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;

                        _context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetaislFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetaislFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetaislFromDb.CartDetailsId;
                        _context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();


                    }
                }
                _response.Result = cartDto;



            }
            catch (Exception ex)
            {

                _response.Message = ex.Message.ToString();
                _response.IsSucess = false;
            }

            return _response;

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {

            try
            {

                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_context.CartHeaders.First(u => u.UserId == userId)),

                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_context.CartDetails
                                                                            .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _productServices.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.ProductDto = productDtos.FirstOrDefault( u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.ProductDto.Price);
                }
                //apply coupon if any
                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
                _response.IsSucess = true;   
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSucess = false;
            }

            return _response;

        }


        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int CartDetailsId)
        {
            try
            {

                CartDetails cartDetails = _context.CartDetails
                    .First(u => u.CartDetailsId == CartDetailsId);

                int totalCountOfCartItem = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetails);

                if(totalCountOfCartItem == 1)
                {

                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _context.CartHeaders.Remove(cartHeaderToRemove);

                }

                await _context.SaveChangesAsync();

                _response.Result = true;


            }
            catch (Exception ex)
            {

                _response.Message = ex.Message.ToString();
                _response.IsSucess = false;
            }

            return _response;
        }




    }
}
