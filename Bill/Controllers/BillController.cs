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

            var quantities = billEntites.Select(quantity => quantity.Quantity);
            var eachQuantities = quantities.FirstOrDefault();

            var catalogItemEntites = await CatalogItemRepository.GetAllAsync(filter: item => eachItems.Contains(item.Id));


            var billDto = billEntites.Select(billItem =>
            {
                List<CatalogItem> catalogItems = new List<CatalogItem>();
                decimal totalPrice = 0;
                for (int i = 0; i < eachItems?.Count(); i++)
                {
                    var catalogItem = catalogItemEntites.Single(predicate: catalogItem => catalogItem.Id == billItem.CatalogItemId[i]);
                    catalogItems.Add(catalogItem);
                    totalPrice += catalogItem.Price * eachQuantities[i];
                }



                return billItem.AsDto(totalPrice, catalogItems, eachQuantities);
            });
            return Ok(billDto);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var billItems = await BillRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId && item.CatalogItemId == grantItemDto.CatalogItemId);

            if( grantItemDto.Quantity == null || grantItemDto.Quantity.Count != grantItemDto.CatalogItemId.Count)
            {
                return BadRequest();
            }
            if (billItems == null )
            {
                for(int i = 0; i < grantItemDto.Quantity.Count; i++)
                {
                    if (grantItemDto.Quantity[i] ==0)
                    {
                        return BadRequest();
                    }
                }
                {
                    billItems = new Bills
                    {
                        UserId = grantItemDto.UserId,
                        CatalogItemId = grantItemDto.CatalogItemId,
                        Quantity = grantItemDto.Quantity,
                        CreatedDate = DateTimeOffset.Now,
                    };

                    for (int i = 0; i < billItems.CatalogItemId.Count(); i++)
                    {
                        var catalogItemEntites = await CatalogItemRepository.GetAsync(billItems.CatalogItemId[i]);
                        billItems.TotalPrice += catalogItemEntites.Price * billItems.Quantity[i];
                    }
                    await BillRepository.CreateAsync(billItems);
                }
            }


            return Ok();
        }
        /*[HttpPut("{CatalogItemId, Quantity}")]
        public async Task<ActionResult> PostOneItem(Guid CatalogItemId, int Quantity)
        {

            return Ok();
        }
*/
        [HttpPut("{billId}")]
        public async Task<IActionResult> PutAsync(Guid billId, UpdateBillDto updateBillDto)
        {
            var existingBill = await BillRepository.GetAsync(billId);

            if(existingBill == null)
            {
                return NotFound();
            }

            
            existingBill.CatalogItemId = updateBillDto.CatalogItemId;
            existingBill.Quantity = updateBillDto.Quantity;

            for (int i = 0; i < existingBill.CatalogItemId.Count(); i++)
            {
                var catalogItemEntites = await CatalogItemRepository.GetAsync(existingBill.CatalogItemId[i]);
                existingBill.TotalPrice += catalogItemEntites.Price * existingBill.Quantity[i];
            }
            existingBill.CreatedDate = DateTimeOffset.Now;
            await BillRepository.UpdateAsync(existingBill);

            return NoContent();
        }
        

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid billId)
        {
            var Bill = await BillRepository.GetAsync(billId);

            if(Bill == null)
            {
                return NotFound();
            }

            await BillRepository.RemoveAsync(Bill.Id);

            return NoContent();
            

        }
    }
}
