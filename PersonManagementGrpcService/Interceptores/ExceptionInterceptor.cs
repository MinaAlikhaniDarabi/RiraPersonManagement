using Grpc.Core;
using Grpc.Core.Interceptors;
using RiraPersonManagement;


public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
            var errorResponse = new ErrorResponse
            {
                ErrorMessage = ex.Status.Detail,
                ErrorCode = ex.StatusCode.ToString()
            };

            return (TResponse)(object)new PersonServiceResponse { Error = errorResponse };

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex}");

            var errorResponse = new ErrorResponse
            {
                ErrorMessage = "An internal error occurred",
                ErrorCode = "INTERNAL_ERROR"
            };

            return (TResponse)(object)new PersonServiceResponse { Error = errorResponse };

        }
    }
}