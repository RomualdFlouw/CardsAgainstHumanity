De intentie van deze web applicatie is om het kaartspel "Cards against humanity" online te kunnen spelen met je vrienden.

Spelregels:

Elke speler raapt tien witte kaarten. Iedere ronde is 1 iemand Card Czar en speelt deze een zwarte kaart. De andere spelers beantwoorden de kaart met een witte kaart en spelen deze door naar de Card Czar. De Card Czar schudt de antwoorden en deelt de kaartcombinaties met de groep. De Card Czar kiest de grappigste combinatie uit en de winnaar krijgt een punt. Na elke ronde raapt iedereen een nieuwe witte kaart en wordt de rol van Card Czar doorgeschoven.

Realtime gedeelte:

1) Als iemand op zijn scherm een kaart kiest zal deze direct getoond worden op de Card Czar zijn scherm
2) Chatroom

API:
We kunnen onze API opvullen met zelf gemaakte kaarten of we kunnen een externe api gebruiken om onze kaarten aan te vullen (https://crhallberg.com/cah/). Aan de hand van deze API kunnen we voorgemaakte decks selecteren en deze toevoegen aan onze database. Zo hoeven we minder eigen data te maken en sneller beginnen aan onze web applicatie.

Dosier:
  - Praktische elementen: wij hebben geen users, je moet gewoon zorgen dat de signalr server aanstaat dan zou u moeten kunnen spelen.
  - aanvulling backend: goed idee, wij hebben geen users nodig, geen api calls, enkel signalr
  - rapportering: 
      grootste succes: gamecontroller, signalr laten werken
      grootste moeilijkheid: het spel zelf uitdenken hoe we het eigenlijk moeten doen met signalr en frontend
      
      
