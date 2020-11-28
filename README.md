# Azure-Sample-EventHub
The idea is to emit custom events to an EventHub and receive those events from a consumer group. There are two console projects (SendEvent, ReceiveEvent) in this solution. A blob storage account is required to read events from EventHub. Azure SDK will save checkpoint information in a blob storage. A checkpoint keeps track of how many events that are read from a eventHub queue.


<img src="Architecture.jpg" />
