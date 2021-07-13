namespace Market.Business.Contracts
{
    public interface IOrderVendor<R, T>
    {
        public T CreateOrder(R request);
    }
}
