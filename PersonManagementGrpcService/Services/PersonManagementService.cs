using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
        int newId = _persons.Count > 0 ? _persons.Max(p => p.Id) + 1 : 1;

        var newPerson = new Person
        {
            Id = newId,
            Name = request.Person.Name,
            LastName = request.Person.LastName,
            NationalCode = request.Person.NationalCode,
            BirthDate = request.Person.BirthDate
        };

        var person = _persons.FirstOrDefault(p => p.NationalCode == request.Person.NationalCode);
        if(person!=null)           
        throw new RpcException(new Status(StatusCode.NotFound, "person already added"));

        _persons.Add(newPerson);

        return Task.FromResult(new PersonServiceResponse
        {
            Data = Any.Pack(new CreatePersonResponse { Person = newPerson })
        });
    }

    // UpdatePerson: Updates an existing person
    public override Task<PersonServiceResponse> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
            var existingPerson = _persons.FirstOrDefault(p => p.Id == request.Person.Id);
            if (existingPerson == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));
            }

            existingPerson.Name = request.Person.Name;
            existingPerson.LastName = request.Person.LastName;
            existingPerson.NationalCode = request.Person.NationalCode;
            existingPerson.BirthDate = request.Person.BirthDate;

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