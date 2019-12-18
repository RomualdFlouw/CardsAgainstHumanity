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
      connection.on('ReceiveStartingHand', (hand) => {
        gameHub.$emit('receive-starting-hand', hand)
      })

      connection.on('ReceiveRoundInfo', (info) => {
        gameHub.$emit('receive-round-info', info)
      })

      connection.on('PickWinner', (cards) => {
        gameHub.$emit('pick-winner', cards)
      })

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

    gameHub.JoinGame = (userName) => {
      if (!startedPromise) 
          return;

    return startedPromise
      .then(() => connection.invoke("JoinGame", userName))
      .catch(console.error);
    };

    gameHub.GetStartingHand = () => {
      if (!startedPromise) 
          return;

      return startedPromise
        .then(() => connection.invoke("GetStartingHand"))
        .catch(console.error);
    };

    gameHub.GetRoundInfo = () => {
      if (!startedPromise) 
          return;

      return startedPromise
        .then(() => connection.invoke("GetRoundInfo"))
        .catch(console.error);
    };

    gameHub.ClientSelectedCard = (card) => {
      if (!startedPromise) 
          return;

      return startedPromise
        .then(() => connection.invoke("ClientSelectedCard", card))
        .catch(console.error);
    };

    
    // Provide methods for components to send messages back to server
    // Make sure no invocation happens until the connection is established
  }
};
