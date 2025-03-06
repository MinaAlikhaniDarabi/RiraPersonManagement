using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using RiraPersonManagement;
using PersonManagementGrpcClient;

class Program
{
    static async Task Main(string[] args)
    {
        var personServiceAddress = "http://localhost:5011";
        using var channel = GrpcChannel.ForAddress(personServiceAddress);
        var client = new PersonService.PersonServiceClient(channel);


        #region CreatPerson Test

        var CreatePersonRequest = new CreatePersonRequest()
        {
            Person = new Person()
            {
                Id = 0,
                Name = "Mina",
                LastName = "AlikhaniDarabi",
                NationalCode = "2280849518",
                BirthDate = Timestamp.FromDateTime(DateTime.UtcNow)
            }
        };
        var createResponse = await GrpcHelper.CallServiceAsync<CreatePersonResponse>(async () => await client.CreatePersonAsync(CreatePersonRequest), nameof(client.CreatePersonAsync));
        if (createResponse != null)
            Console.WriteLine($"Created Person: {createResponse.Person.Name} {createResponse.Person.LastName}");

        #endregion

        #region GetPerson Test

        var getPersonRequest = new GetPersonRequest() { Id = 1 };
        var getResponse = await GrpcHelper.CallServiceAsync<GetPersonResponse>(async () => await client.GetPersonAsync(getPersonRequest), nameof(client.GetPersonAsync));
        if (getResponse != null)
            Console.WriteLine($"Retrieved Person: {getResponse.Person.Name} {getResponse.Person.LastName}");

        #endregion

        #region GetAllPersons Test

        var getAllResponse = await GrpcHelper.CallServiceAsync<GetAllPersonsResponse>(async () => await client.GetAllPersonsAsync(new Empty()), nameof(client.GetAllPersonsAsync));
        if (getAllResponse != null)
        {
            Console.WriteLine("All Persons:");
            foreach (var person in getAllResponse.Persons)
            {
                Console.WriteLine($"{person.Name} {person.LastName} (ID: {person.Id})");
            }
        }

        #endregion

        #region UpdatePerson Test
        var updatePersonRequest = new UpdatePersonRequest()
        {
            Person = new Person()
            {
                Id = 1,
                Name = "Minajoon",
                LastName = "Alikhani",
                NationalCode = "2280849518",
                BirthDate = Timestamp.FromDateTime(DateTime.UtcNow)
            }
        };

        var updateResponse = await GrpcHelper.CallServiceAsync<UpdatePersonResponse>(async () => await client.UpdatePersonAsync(updatePersonRequest), nameof(client.UpdatePersonAsync));
        if (updateResponse != null)
            Console.WriteLine($"Updated Person: {updateResponse.Person.Name} {updateResponse.Person.LastName}");

        #endregion

        #region DeletePerson Test

        var deletePersonRequest = new DeletePersonRequest() { Id = 1 };
        var deleteResponse = await GrpcHelper.CallServiceAsync<DeletePersonResponse>(async () => await client.DeletePersonAsync(deletePersonRequest), nameof(client.DeletePersonAsync));
        if (deleteResponse != null)
            Console.WriteLine($"Delete Success: {deleteResponse.Success}");
        #endregion

        Console.ReadLine();
    }

}

