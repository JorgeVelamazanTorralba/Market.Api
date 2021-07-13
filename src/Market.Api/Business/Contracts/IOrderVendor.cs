namespace Market.Api.Business.Contracts
{
    public interface IOrderVendor<R, T>
    {
        public T CreateOrder(R request);
    }
}
