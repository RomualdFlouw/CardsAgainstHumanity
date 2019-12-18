import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

export default {
  install(Vue) {
    const lobbyHub = new Vue();
    Vue.prototype.$lobbyHub = lobbyHub;

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null;
    let startedPromise = null;
    let manuallyClosed = false;

    Vue.prototype.startSignalR = jwtToken => {
      connection = new HubConnectionBuilder()
        .withUrl(
          `https://localhost:44395/lobbyhub`,
          jwtToken ? { accessTokenFactory: () => jwtToken } : null
        )
        .configureLogging(LogLevel.Information)
        .build();

      // Forward hub events through the event, so we can listen for them in the Vue components
      connection.on('UserAdded', (user) => {
        lobbyHub.$emit('user-added', user)
      })
      connection.on('UpdateLobby', (users) => {
        lobbyHub.$emit('update-lobby', users)
      })
      connection.on('UsernameChecked', (isAvailable) => {
        lobbyHub.$emit('username-checked', isAvailable)
      })
      connection.on('LobbyFull', () => {
        lobbyHub.$emit('lobby-full')
      })
      connection.on('UpdateReadyState', () => {
        lobbyHub.$emit('update-ready-state')
      })
      connection.on('StartCountdownToGame', () => {
        lobbyHub.$emit('start-countdown-game')
      })
      connection.on('StopCountdownToGame', () => {
        lobbyHub.$emit('stop-countdown-game')
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
    Vue.prototype.stopSignalR = () => {
      if (!startedPromise) return;

      manuallyClosed = true;
      return startedPromise
        .then(() => connection.stop())
        .then(() => {
          startedPromise = null;
        });
    };

    lobbyHub.LoginUser = (user) => {
        if (!startedPromise) 
            return;

      return startedPromise
        .then(() => connection.invoke("LoginUser", user))
        .catch(console.error);
    };

    lobbyHub.GetAllPlayers = () => {
      if (!startedPromise) 
          return;

      return startedPromise
        .then(() => connection.invoke("GetAllPlayers"))
        .catch(console.error);
    };

    lobbyHub.ClientReadyStateChange = (user) => {
      if (!startedPromise) 
          return;

      return startedPromise
        .then(() => connection.invoke("ClientReadyStateChange"))
        .catch(console.error);
    };
    // Provide methods for components to send messages back to server
    // Make sure no invocation happens until the connection is established
  }
};
