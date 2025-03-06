using Grpc.Core;

namespace PersonManagementGrpcService.Helpers
{
    public static class IdentityHelper
    {
        public static void ValidateNationalCode(string nationalCode)
        {
            if (string.IsNullOrEmpty(nationalCode))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code cannot be null or empty."));
            }

            if (nationalCode.Length != 10)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code must be exactly 10 digits."));
            }

            if (!nationalCode.All(char.IsDigit))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National Code must contain only digits."));
            }
        }
    }
}
