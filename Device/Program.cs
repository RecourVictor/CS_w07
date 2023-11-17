using System.Text;
using Microsoft.Azure.Devices.Client;
using Howest.MCT.Models;
using Newtonsoft.Json;

var connectionString = "HostName=CSiotvictorrecour.azure-devices.net;DeviceId=pcvictor;SharedAccessKey=fUSxdMNwgMMQqzvDmDdhzd0Q7WrkwyWlCAIoTJ4qTnk=";

using var deviceClient = DeviceClient.CreateFromConnectionString(connectionString);


//// open connection explicitly
await deviceClient.OpenAsync();

await deviceClient.SetReceiveMessageHandlerAsync(ReceiveMessage, null);

async Task ReceiveMessage(Message message, object userContext)
{
    var messageData = Encoding.ASCII.GetString(message.GetBytes());
    Console.WriteLine("Received message: {0}", messageData);
    await deviceClient.CompleteAsync(message);
}

while (true)
{
    await SendMessage();
    Thread.Sleep(5000);
}


async Task SendMessage()
{
    Sensordata sensordata = new Sensordata()
    {
        SensorValue = 42
    };

    var jsonData = JsonConvert.SerializeObject(sensordata);
    var bytes = Encoding.UTF8.GetBytes(jsonData);
    var message = new Message(bytes);

    await deviceClient.SendEventAsync(message);

    Console.WriteLine("A single message is sent");
}

