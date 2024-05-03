using Bill.Dto;
using Bill.Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;

namespace Bill.Controllers
{
    [ApiController]
    [Route("bill")]
    public class BillController : ControllerBase
    {
        private readonly IRepository<Bills> BillRepository;
        private readonly IRepository<CatalogItem> CatalogItemRepository;

        public BillController(IRepository<Bills> BillRepository, IRepository<CatalogItem> CatalogItemRepository)
        {
            this.BillRepository = BillRepository;
            this.CatalogItemRepository = CatalogItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var billEntites = await BillRepository.GetAllAsync(bill => bill.UserId == userId);
            var itemIds = billEntites.Select(itemlist => itemlist.CatalogItemId);

            var eachItems = itemIds.FirstOrDefault();

            var catalogItemEntites = await CatalogItemRepository.GetAllAsync(item => eachItems.Contains(item.Id));

            
            var billDto = billEntites.Select(billItem =>
            {
                List<CatalogItem> catalogItems = new List<CatalogItem>();
                decimal totalPrice = 0;
                for (int i = 0; i < eachItems?.Count(); i++)
                {
                    var catalogItem = catalogItemEntites.Single(predicate: catalogItem => catalogItem.Id == billItem.CatalogItemId[i]);
                    catalogItems.Add(catalogItem);
                    totalPrice += catalogItem.Price;
                }
                
                

                return billItem.AsDto(totalPrice, catalogItems);
            });
            return Ok(billDto);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var billItems = await BillRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId && item.CatalogItemId == grantItemDto.CatalogItemId);

            if(billItems == null)
            {
                billItems = new Bills
                {
                    CatalogItemId = grantItemDto.CatalogItemId,
                };
                await BillRepository.CreateAsync(billItems);
            }


            return Ok();
        }
    }
}
