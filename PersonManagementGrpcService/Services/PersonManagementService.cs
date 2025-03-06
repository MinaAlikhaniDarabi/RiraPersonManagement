using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PersonManagementGrpcService.Helpers;
using RiraPersonManagement;


public class PersonManagementService : PersonService.PersonServiceBase
{
    private static readonly List<Person> _persons = new List<Person>();

    // GetPerson: Retrieves a single person by ID
    public override Task<PersonServiceResponse> GetPerson(GetPersonRequest request, ServerCallContext context)
    {
      
            var person = _persons.FirstOrDefault(p => p.Id == request.Id);
            if (person == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));
            }

            return Task.FromResult(new PersonServiceResponse
            {
                Data = Any.Pack(new GetPersonResponse { Person = person })
            });
    
    }

    // GetAllPersons: Retrieves all persons
    public override Task<PersonServiceResponse> GetAllPersons(Empty request, ServerCallContext context)
    {        
            var response = new PersonServiceResponse
            {
                Data = Any.Pack(new GetAllPersonsResponse { Persons = { _persons } })
            };

            return Task.FromResult(response);
    }

    // CreatePerson: Creates a new person
    public override Task<PersonServiceResponse> CreatePerson(CreatePersonRequest request, ServerCallContext context)
    {
        if(request.Person==null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "person is required"));

        var name = request.Person.Name;
        var LastName = request.Person.LastName;
        var nationalCode = request.Person.NationalCode;
        var birthDate = request.Person.BirthDate;

        IdentityHelper.ValidateNationalCode(nationalCode);

        int newId = _persons.Count > 0 ? _persons.Max(p => p.Id) + 1 : 1;
        var newPerson = new Person
        {
            Id = newId,
            Name = name,
            LastName = LastName,
            NationalCode = nationalCode,
            BirthDate = birthDate
        };
        var person = _persons.FirstOrDefault(p => p.NationalCode == nationalCode);
        if(person!=null)           
        throw new RpcException(new Status(StatusCode.AlreadyExists, "person already added"));

        _persons.Add(newPerson);

        return Task.FromResult(new PersonServiceResponse
        {
            Data = Any.Pack(new CreatePersonResponse { Person = newPerson })
        });
    }

    // UpdatePerson: Updates an existing person
    public override Task<PersonServiceResponse> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
        if (request.Person == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "person is required"));

        var id = request.Person.Id;
        var name = request.Person.Name;
        var LastName = request.Person.LastName;
        var nationalCode = request.Person.NationalCode;
        var birthDate = request.Person.BirthDate;

        IdentityHelper.ValidateNationalCode(nationalCode);

        var existingPerson = _persons.FirstOrDefault(p => p.Id == request.Person.Id);
            if (existingPerson == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));
            }



            existingPerson.Name = name;
            existingPerson.LastName = LastName;
            existingPerson.NationalCode = nationalCode;
            existingPerson.BirthDate = birthDate;

            return Task.FromResult(new PersonServiceResponse
            {
                Data = Any.Pack(new UpdatePersonResponse { Person = existingPerson })
            });
    }

    // DeletePerson: Deletes a person by ID
    public override Task<PersonServiceResponse> DeletePerson(DeletePersonRequest request, ServerCallContext context)
    {
      var person = _persons.FirstOrDefault(p => p.Id == request.Id);
            if (person == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));
            }

            _persons.Remove(person);

            return Task.FromResult(new PersonServiceResponse
            {
                Data = Any.Pack(new DeletePersonResponse { Success = true })
            });
     
    }
}