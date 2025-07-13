using NUnit.Framework;
using Rossoforge.Core.Events;
using Rossoforge.Events.Service;
using System.Collections.Generic;

namespace Rossoforge.Events.Tests
{
    [TestFixture]
    public class EventServiceTests
    {
        private EventService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new EventService();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Dispose();
        }

        // Evento de prueba
        private class TestEvent : IEvent
        {
            public int Value { get; set; }
        }

        // Listener de prueba
        private class TestListener : IEventListener<TestEvent>
        {
            public List<TestEvent> ReceivedEvents { get; } = new();

            public void OnEventInvoked(TestEvent e)
            {
                ReceivedEvents.Add(e);
            }
        }

        [Test]
        public void RegisterListener_ThenRaise_EventIsReceived()
        {
            var listener = new TestListener();
            _service.RegisterListener(listener);

            var testEvent = new TestEvent { Value = 42 };
            _service.Raise(testEvent);

            Assert.AreEqual(1, listener.ReceivedEvents.Count);
            Assert.AreEqual(42, listener.ReceivedEvents[0].Value);
        }

        [Test]
        public void UnregisterListener_ThenRaise_EventIsNotReceived()
        {
            var listener = new TestListener();
            _service.RegisterListener(listener);
            _service.UnregisterListener(listener);

            _service.Raise(new TestEvent { Value = 100 });

            Assert.AreEqual(0, listener.ReceivedEvents.Count);
        }

        [Test]
        public void Raise_WithNoListeners_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
            {
                _service.Raise(new TestEvent());
            });
        }

        [Test]
        public void Raise_WithoutArgument_UsesDefaultInstance()
        {
            var listener = new TestListener();
            _service.RegisterListener(listener);

            _service.Raise<TestEvent>();

            Assert.AreEqual(1, listener.ReceivedEvents.Count);
        }

#if UNITY_EDITOR
        [Test]
        public void GetAllBuses_ReturnsCorrectCount()
        {
            var listener = new TestListener();
            _service.RegisterListener(listener);

            var buses = _service.GetAllBuses();

            Assert.AreEqual(1, buses.Length);
        }
#endif
    }
}
