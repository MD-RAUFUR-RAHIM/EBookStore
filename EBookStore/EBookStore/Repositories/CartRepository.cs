using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace EBookStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public  async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using  var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _applicationDbContext.ShoppingCarts.Add(cart);
                }
                _applicationDbContext.SaveChanges();
                //cart details section
                var cartItem = _applicationDbContext.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if(cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _applicationDbContext.CartDetails.Add(cartItem);
                }
                _applicationDbContext.SaveChanges();
                transaction.Commit();
                
            }
            catch (Exception ex) {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Invalid Cart");
                }   
                //cart details section
                var cartItem = _applicationDbContext.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem == null)
                {
                    throw new Exception("No Items in cart");
                }
                else if(cartItem.Quantity == 1)
                {
                    _applicationDbContext.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                    _applicationDbContext.CartDetails.Add(cartItem);
                }
                _applicationDbContext.SaveChanges();       
                
            }
            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
       
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if(userId == null)
                throw new Exception("Invalid userId");
            
            var shoppingCart = await _applicationDbContext.ShoppingCarts
                                                    .Include(a => a.CartDetails)
                                                    .ThenInclude(a => a.Book)
                                                    .ThenInclude(a => a.Genre)
                                                    .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;                                        
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _applicationDbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        
        public async Task<int> GetCartItemCount(string userId="")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _applicationDbContext.ShoppingCarts
                        join cartDetail in _applicationDbContext.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
