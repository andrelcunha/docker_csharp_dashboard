using Docker.DotNet.Models;
using System.Runtime.Serialization;

namespace ALC.Docker.Manager.API.Models;

    public enum Environment {
        Docker,
        Podman,
    }

[DataContract]
public class Container : ContainerListResponse
{
    private readonly Environment _environment;
    
    [DataMember(Name = "Environment", EmitDefaultValue = false)]
    public string Environment
    {
        get
        {
            return _environment.ToString();
        }
    }

    public Container(ContainerListResponse response, Environment environment = (Environment)1)
    {
        foreach (var property in typeof(ContainerListResponse).GetProperties())
        {
            property.SetValue(this, property.GetValue(response));
        }
        _environment = environment;
    }
}
