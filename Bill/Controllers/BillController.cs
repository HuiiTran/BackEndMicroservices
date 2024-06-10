using Bill.Dto;
using Bill.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesCommon;
using BillUpdateCatalogItem;
using SoldQuantityUpdate;

namespace Bill.Controllers
{
    [ApiController]
    [Route("bill")]
    public class BillController : ControllerBase
    {
        private readonly IRepository<Bills> BillRepository;
        private readonly IRepository<CatalogItem> CatalogItemRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public BillController(IRepository<Bills> BillRepository, IRepository<CatalogItem> CatalogItemRepository, IPublishEndpoint publishEndpoint)
        {
            this.BillRepository = BillRepository;
            this.CatalogItemRepository = CatalogItemRepository;
            this.publishEndpoint = publishEndpoint;
    }
        [HttpGet("{billId}")]
        public async Task<ActionResult<IEnumerable<BillItemDto>>> GetBillById(Guid billId)
        {

            var billEntites = await BillRepository.GetAllAsync(bill => bill.Id == billId);

            var itemIds = billEntites.Select(itemlist => itemlist.CatalogItemId);
            // var eachItems = itemIds.FirstOrDefault();

            /*var quantities = billEntites.Select(quantity => quantity.Quantity);
            var eachQuantities = quantities.FirstOrDefault();
*/
            //var catalogItemEntites = await CatalogItemRepository.GetAllAsync(filter: item => eachItems.Contains(item.Id));

            IEnumerable<Task<BillItemDto>>? billDto = null;


            billDto = billEntites.Select(async billItem =>
            {
                List<CatalogItem> catalogItems = new List<CatalogItem>();

                foreach (var eachItems in itemIds)
                {

                    var catalogItemEntites = await CatalogItemRepository.GetAllAsync(item => eachItems.Contains(item.Id));
                    //catalogItems =  new List<CatalogItem>(); 

                    for (int i = 0; i < eachItems?.Count(); i++)
                    {
                        try
                        {
                            var catalogItem = catalogItemEntites.Single(catalogItem => catalogItem.Id == billItem.CatalogItemId[i]);
                            catalogItems.Add(catalogItem);
                        }
                        catch
                        {

                        }
                    }
                }
                return billItem.AsDto(catalogItems);
            });
            return Ok(billDto);

        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BillItemDto>>> GetAllAsync()
        {

            var billEntites = await BillRepository.GetAllAsync();

            var itemIds = billEntites.Select(itemlist => itemlist.CatalogItemId);
            // var eachItems = itemIds.FirstOrDefault();

            /*var quantities = billEntites.Select(quantity => quantity.Quantity);
            var eachQuantities = quantities.FirstOrDefault();
*/
            //var catalogItemEntites = await CatalogItemRepository.GetAllAsync(filter: item => eachItems.Contains(item.Id));

            IEnumerable<Task<BillItemDto>>? billDto = null;


            billDto = billEntites.Select(async billItem =>
            {
                List<CatalogItem> catalogItems = new List<CatalogItem>();

                foreach (var eachItems in itemIds)
                {

                    var catalogItemEntites = await CatalogItemRepository.GetAllAsync(item => eachItems.Contains(item.Id));
                    //catalogItems =  new List<CatalogItem>(); 

                    for (int i = 0; i < eachItems?.Count(); i++)
                    {
                        try
                        {
                            var catalogItem = catalogItemEntites.Single(catalogItem => catalogItem.Id == billItem.CatalogItemId[i]);
                            catalogItems.Add(catalogItem);
                        }
                        catch
                        {

                        }
                    }
                }
                return billItem.AsDto(catalogItems);
            });
            return Ok(billDto);

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
            // var eachItems = itemIds.FirstOrDefault();

            /*var quantities = billEntites.Select(quantity => quantity.Quantity);
            var eachQuantities = quantities.FirstOrDefault();
*/
            //var catalogItemEntites = await CatalogItemRepository.GetAllAsync(filter: item => eachItems.Contains(item.Id));

            IEnumerable<Task<BillItemDto>>? billDto = null;
            

                billDto = billEntites.Select(async billItem =>
                {
                    List<CatalogItem> catalogItems = new List<CatalogItem>();

                    foreach (var eachItems in itemIds)
                    {

                        var catalogItemEntites = await CatalogItemRepository.GetAllAsync(item => eachItems.Contains(item.Id));
                        //catalogItems =  new List<CatalogItem>(); 

                        for (int i = 0; i < eachItems?.Count(); i++)
                        {
                            try
                            {
                                var catalogItem = catalogItemEntites.Single(catalogItem => catalogItem.Id == billItem.CatalogItemId[i]);
                                catalogItems.Add(catalogItem);
                            }
                            catch { 
                                
                            }
                        }
                    }
                    return billItem.AsDto(catalogItems);
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
            
                for(int i = 0; i < grantItemDto.Quantity.Count; i++)
                {
                    if (grantItemDto.Quantity[i] == 0)
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
                        State = grantItemDto.State,
                        Address = grantItemDto.Address,
                        Phone = grantItemDto.Phone,
                    };

                    for (int i = 0; i < grantItemDto.Quantity.Count; i++)
                    {
                        var existingCatalogItem = await CatalogItemRepository.GetAsync(grantItemDto.CatalogItemId[i]);
                        existingCatalogItem.Quantity -= grantItemDto.Quantity[i];
                        await CatalogItemRepository.UpdateAsync(existingCatalogItem);
                        await publishEndpoint.Publish(new BillCatalogItemUpdated(existingCatalogItem.Id, existingCatalogItem.Quantity));
                    await publishEndpoint.Publish(new UpdateSoldQuantity(existingCatalogItem.Id, (grantItemDto.Quantity[i])));
                }

                    for (int i = 0; i < billItems.CatalogItemId.Count(); i++)
                    {
                        var catalogItemEntites = await CatalogItemRepository.GetAsync(billItems.CatalogItemId[i]);
                        billItems.TotalPrice += catalogItemEntites.Price * billItems.Quantity[i];
                    }
                    await BillRepository.CreateAsync(billItems);
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
            for (int i = 0; i < existingBill.Quantity.Count; i++)
            {
                var existingCatalogItem = await CatalogItemRepository.GetAsync(existingBill.CatalogItemId[i]);
                if(existingBill.State != "Cancel" && updateBillDto.State == "Cancel")
                {
                    existingCatalogItem.Quantity += existingBill.Quantity[i];
                    await CatalogItemRepository.UpdateAsync(existingCatalogItem);
                    await publishEndpoint.Publish(new BillCatalogItemUpdated(existingCatalogItem.Id, existingCatalogItem.Quantity));
                    await publishEndpoint.Publish(new UpdateSoldQuantity(existingCatalogItem.Id, -(existingBill.Quantity[i])));
                }
                else if (existingBill.State != "Cancel" && updateBillDto.State != "Cancel")
                {
                   
                }
                else if (existingBill.State == "Cancel" && updateBillDto.State != "Cancel")
                {
                    existingCatalogItem.Quantity -= existingBill.Quantity[i];
                    await CatalogItemRepository.UpdateAsync(existingCatalogItem);
                    await publishEndpoint.Publish(new BillCatalogItemUpdated(existingCatalogItem.Id, existingCatalogItem.Quantity));
                    await publishEndpoint.Publish(new UpdateSoldQuantity(existingCatalogItem.Id, (existingBill.Quantity[i])));

                }
            }

            existingBill.State = updateBillDto.State;
            /*existingBill.CatalogItemId = updateBillDto.CatalogItemId;
            existingBill.Quantity = updateBillDto.Quantity;

            for (int i = 0; i < existingBill.CatalogItemId.Count(); i++)
            {
                var catalogItemEntites = await CatalogItemRepository.GetAsync(existingBill.CatalogItemId[i]);
                existingBill.TotalPrice += catalogItemEntites.Price * existingBill.Quantity[i];
            }
            existingBill.CreatedDate = DateTimeOffset.Now;*/
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
