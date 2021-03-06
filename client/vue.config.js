// Inside vue.config.js
module.exports = {
    devServer: {
        port: 8080
      },
    // ...other vue-cli plugin options...
    pwa: {
      name: 'CardsAgainstHumanity',
      themeColor: '#333335',
      msTileColor: '#10CCFF',
      appleMobileWebAppCapable: 'yes',
      appleMobileWebAppStatusBarStyle: '#333335',
      icons: [
        {
            "src": "/favicon-32x32.png",
            "sizes": "32x32",
            "type": "image/png"
        },        {
            "src": "/favicon-16x16.png",
            "sizes": "16x16",
            "type": "image/png"
        },        {
            "src": "/apple-touch-icon-152x152.png",
            "sizes": "152x152",
            "type": "image/png"
        },        {
            "src": "/safari-pinned-tab.svg",
            "type": "image/svg"
        },        {
            "src": "/msapplication-icon-144x144.png",
            "sizes": "144x144",
            "type": "image/png"
        },
        {
            "src": "/android-chrome-192x192.png",
            "sizes": "192x192",
            "type": "image/png"
        },
        {
            "src": "/android-chrome-512x512.png",
            "sizes": "512x512",
            "type": "image/png"
        }
    ],
    backgroundColor: "#333335",
      }
    }   
  