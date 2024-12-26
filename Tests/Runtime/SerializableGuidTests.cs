using NUnit.Framework;

namespace LeonDrace.Utility.Tests
{
    public class SerializableGuidTests
    {
        [Test]
        public void CreateTwoUniqueGuids()
        {
            var guid1 = new SerializableGuid();
            var guid2 = new SerializableGuid();
            
            Assert.That(guid1, Is.Not.Null);
            Assert.That(guid2, Is.Not.Null);
            Assert.AreNotEqual(guid1, guid2);
        }
        
        [Test]
        public void CompareEqualOperator()
        {
            var guid1 = new SerializableGuid();
            var guid2 = new SerializableGuid(guid1.Id);
            
            Assert.That(guid1, Is.Not.Null);
            Assert.That(guid2, Is.Not.Null);
            Assert.That(guid1.Equals(guid2));
            Assert.That(guid1 == guid2);
        }
        
        [Test]
        public void CompareUnEqualOperator()
        {
            var guid1 = new SerializableGuid();
            var guid2 = new SerializableGuid();
            
            Assert.That(guid1, Is.Not.Null);
            Assert.That(guid2, Is.Not.Null);
            Assert.That(!guid1.Equals(guid2));
            Assert.That(guid1 != guid2);
        }
        
        [Test]
        public void CompareInstanceToNull()
        {
            var guid1 = new SerializableGuid();
            SerializableGuid guid2 = null;
            
            Assert.That(guid1, Is.Not.Null);
            Assert.That(guid2, Is.Null);
            Assert.That(!guid1.Equals(guid2));
        }
        
        [Test]
        public void CompareNullToInstance()
        {
            var guid1 = new SerializableGuid();
            SerializableGuid guid2 = null;
            
            Assert.That(guid1, Is.Not.Null);
            Assert.That(guid2, Is.Null);
            Assert.That(guid2 != guid1);
        }
    }
}