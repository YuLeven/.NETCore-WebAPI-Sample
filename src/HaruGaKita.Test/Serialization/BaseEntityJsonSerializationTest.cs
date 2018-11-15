using System;
using HaruGaKita.Domain.Entities;
using Newtonsoft.Json;
using Xunit;

namespace HaruGaKita.Test.Entities.Serialization
{
  public class BaseEntitySerializationTest
  {
    private readonly BaseEntity _baseEntity;
    private readonly string _serializedEntity;

    public BaseEntitySerializationTest()
    {
      _baseEntity = new BaseEntity()
      {
        Uid = Guid.NewGuid()
      };
      _serializedEntity = JsonConvert.SerializeObject(_baseEntity);
    }

    [Fact]
    public void Serializing_Omits_Internal_Id()
    {
      var deserializedEntity = JsonConvert.DeserializeObject<ExpectedDeserializedEntity>(_serializedEntity);
      
      Assert.DoesNotContain("uid", _serializedEntity);
      Assert.Contains("id", _serializedEntity);
      Assert.True(deserializedEntity.Guid != null);
    }

    public class ExpectedDeserializedEntity
    {
      [JsonProperty("id")]
      public Guid Guid { get; set; }
    }
  }
}