using AutoMapper;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Mappers;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Services;

public class PizzaOrdersService : IPizzaOrdersService
{
    private readonly IPizzaOrderRepository _pizzaOrdersRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<PizzaOrderAddRequest> _pizzaOrderAddValidator;
    private readonly IValidator<PizzaOrderUpdateRequest> _pizzaOrderUpdateValidator;

    public PizzaOrdersService(
        IPizzaOrderRepository pizzaOrdersRepository,
        IMapper mapper,
        IValidator<PizzaOrderAddRequest> pizzaOrderAddValidator,
        IValidator<PizzaOrderUpdateRequest> pizzaOrderUpdateValidator)
    {
        _pizzaOrdersRepository = pizzaOrdersRepository;
        _mapper = mapper;
        _pizzaOrderAddValidator = pizzaOrderAddValidator;
        _pizzaOrderUpdateValidator = pizzaOrderUpdateValidator;
    }

    public async Task<PizzaOrderResponse?> AddPizzaOrder(PizzaOrderAddRequest pizzaOrderAddRequest)
    {
        if (pizzaOrderAddRequest == null)
            throw new ArgumentNullException(nameof(pizzaOrderAddRequest));

        ValidationResult validation = await _pizzaOrderAddValidator.ValidateAsync(pizzaOrderAddRequest);
        if (!validation.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));
        }

        var orderEntity = PizzaOrderMapper.MapToPizzaOrder(pizzaOrderAddRequest);
        var added = await _pizzaOrdersRepository.AddOrderAsync(orderEntity);

        return added != null ? PizzaOrderMapper.MapToPizzaOrderResponse(added) : null;
    }

    public async Task<PizzaOrderResponse?> UpdatePizzaOrder(PizzaOrderUpdateRequest pizzaOrderUpdateRequest)
    {
        if (pizzaOrderUpdateRequest == null)
            throw new ArgumentNullException(nameof(pizzaOrderUpdateRequest));

        ValidationResult validation = await _pizzaOrderUpdateValidator.ValidateAsync(pizzaOrderUpdateRequest);
        if (!validation.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));
        }

        var orderEntity = PizzaOrderMapper.MapToPizzaOrder(pizzaOrderUpdateRequest);
        var updated = await _pizzaOrdersRepository.UpdateOrderAsync(orderEntity);

        return updated == null ? null : PizzaOrderMapper.MapToPizzaOrderResponse(updated);
    }

    public async Task<bool> DeletePizzaOrder(Guid pizzaOrderID)
    {
        return await _pizzaOrdersRepository.DeleteOrderAsync(pizzaOrderID);
    }

    public async Task<PizzaOrderResponse?> GetOrderById(Guid orderId)
    {
        var order = await _pizzaOrdersRepository.GetOrderByIdAsync(orderId);
        return order == null ? null : PizzaOrderMapper.MapToPizzaOrderResponse(order);
    }

    public async Task<List<PizzaOrderResponse>> GetOrders()
    {
        var orders = await _pizzaOrdersRepository.GetAllOrdersAsync();
        return PizzaOrderMapper.MapToPizzaOrderResponseList(orders);
    }

    public async Task<List<PizzaOrderResponse>> GetOrdersByUser(Guid userId, bool onlyOrdered = true)
    {
        var orders = await _pizzaOrdersRepository.GetOrdersByUserAsync(userId, onlyOrdered);
        return _mapper.Map<List<PizzaOrderResponse>>(orders);
    }

    public async Task<PizzaOrderResponse?> GetOrderByCondition(Expression<Func<PizzaOrder, bool>> predicate)
    {
        var order = await _pizzaOrdersRepository.GetPizzaOrderByCondition(predicate);
        return order == null ? null : PizzaOrderMapper.MapToPizzaOrderResponse(order);
    }

    public async Task<List<PizzaOrderResponse>> GetOrdersByCondition(Expression<Func<PizzaOrder, bool>> predicate)
    {
        var orders = await _pizzaOrdersRepository.GetPizzaOrdersByCondition(predicate);
        return PizzaOrderMapper.MapToPizzaOrderResponseList(orders);
    }
}
