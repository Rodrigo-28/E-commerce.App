using E_commerce.application.Dtos.Requests;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            this._orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto, IValidator<CreateOrderDto> validator)
        {
            var vr = await validator.ValidateAsync(createOrderDto);
            if (!vr.IsValid)
                throw new BadRequestException(vr.ToString()) { ErrorCode = "004" };
            var created = await _orderService.Create(createOrderDto);
            return CreatedAtAction(nameof(GetByUser), new { userId = created.UserId }, created);
        }
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _orderService.GetByUser(userId);
            return Ok(list);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(OrderStatus status)
        {
            var list = await _orderService.GetByStatus(status);
            return Ok(list);
        }
        [HttpPut("{id:int}/status")]

        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusDto changeStatus, IValidator<ChangeStatusDto> validator)
        {
            var vr = await validator.ValidateAsync(changeStatus);
            if (!vr.IsValid)
                throw new BadRequestException(vr.ToString()) { ErrorCode = "004" };
            await _orderService.ChangeStatus(id, changeStatus.Status);
            return NoContent();
        }
    }
}
