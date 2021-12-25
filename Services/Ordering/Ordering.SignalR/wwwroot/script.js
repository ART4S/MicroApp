"use strict";

document.getElementById("submit").addEventListener("click", function () {
    const token = document.getElementById("token").value;

    console.log(token);

    if (!token.trim()) return;

    const channel = new signalR.HubConnectionBuilder().withUrl("/notifications", {
        accessTokenFactory: () => token
    }).build();

    channel.on("UpdateOrderStatus", (order) => {
        const li = document.createElement("li");
        li.className = "log";
        li.textContent = JSON.stringify(order);
        document.getElementsByClassName("log-list")[0].appendChild(li);
    });

    channel.start();
});