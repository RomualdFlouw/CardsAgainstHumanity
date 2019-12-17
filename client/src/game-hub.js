import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

export default {
  install(Vue) {
    const gameHub = new Vue();
    Vue.prototype.$gameHub = gameHub;

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null;
    let startedPromise = null;
    let manuallyClosed = false;

    Vue.prototype.startGameSignalR = jwtToken => {
      connection = new HubConnectionBuilder()
        .withUrl(
          `https://localhost:44395/gamehub`,
          jwtToken ? { accessTokenFactory: () => jwtToken } : null
        )
        .configureLogging(LogLevel.Information)
        .build();

      // Forward hub events through the event, so we can listen for them in the Vue components

      // You need to call connection.start() to establish the connection but the client wont handle reconnecting for you!
      // Docs recommend listening onclose and handling it there.
      // This is the simplest of the strategies
      function start() {
        startedPromise = connection.start().catch(err => {
          console.error("Failed to connect with hub", err);
          return new Promise((resolve, reject) =>
            setTimeout(
              () =>
                start()
                  .then(resolve)
                  .catch(reject),
              5000
            )
          );
        });
        return startedPromise;
      }
      connection.onclose(() => {
        if (!manuallyClosed) start();
      });

      // Start everything
      manuallyClosed = false;
      start();
    };
    Vue.prototype.stopGameSignalR = () => {
      if (!startedPromise) return;

      manuallyClosed = true;
      return startedPromise
        .then(() => connection.stop())
        .then(() => {
          startedPromise = null;
        });
    };

    // Provide methods for components to send messages back to server
    // Make sure no invocation happens until the connection is established
  }
};