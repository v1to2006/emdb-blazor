# Elokuvatietokanta projektin määritys

Kehitetään elokuvatietokanta sovellus, joka toimii yhdellä yhteisellä MySQL-tietokannalla.
Sovelluksesta luodaan eri tekniikoilla omat versiot. Jokaisesta tekniikasta vastaa yksi vastuuryhmä, joka hallinnoi tämän tekniikan osuutta. Varsinainen projektin tekeminen on jaettu jokaiselle ryhmälle niin, että kaikki kehittävät kaikkia tekniikoita jonkin verran. Tämä saattaa olla bugien korjailua, hiomista, korjaamista tai ihan vaan uuden kehittämistä.
Kehitystyö dokumentoidaan kattavasti niin, että edellisen ryhmän tekemä työ tulee selvästi esille seuraavalle ryhmälle. Tällöin kehitystyö helpottuu. Myös ToDo-listat ovat hyödyllisiä.
Projektin hallintaan suositellaan käytettävän projektinhallintatyökaluja esim. Trello ja versionhallintaan soveltuvaa ohjelmistoa eli käytännössä Git.

Elokuvatietokantasovelluksessa on tarkoitus päästä luomaan käyttäjälle listaus omista elokuvista. Tämä tarkoittaa kirjautumisjärjestelmää, joka hakee tietokannasta käyttäjän tiedot ja varmentaa salasanan. Salasanan pituus pitää olla vähintään 6 merkki ja sisältää vähintään yhden ison kirjaimen ja yhden erikoismerkin (!#?&%$€£@). Käyttäjältä halutaan vähintään käyttäjänimi, sähköposti. Varmennetaan käyttäjää luodessa, että samalla käyttäjänimellä tai sähköpostilla ei ole jo käyttäjää. Kirjautuminen voidaan tehdä joko käyttäjänimellä tai sähköpostilla.

Sovelluksessa käyttäjä voi lisätä tietokantaan elokuvia joita on katsonut tai joista on kiinnostunut. Käyttäjä voi hakea myös olemassa olevasta tietokannasta elokuvat.

Elokuvista on seuraavat tiedot:
- nimi
- ohjaaja
- vuosi
- kesto (minuutteina)
- genre(t)
- päänäyttelijät

Käyttöliittymät täytyy suunnitella etukäteen, tehkää tekniikalle soveltuvat mock-up piirrokset käyttöliittymistä.
Testivaiheessa MySQL-tietokanta riittää olla omalla paikallisella koneella.

## Tiimit
> [!NOTE]
> Muista varmistaa [aikataulusta](https://docs.google.com/spreadsheets/d/1NEqnXOf7Z1ryIY_puHlkVnVfvkVt-kew2BHigMRRhkQ/edit#gid=0) tiimisi tekniikka.
- Android
   - Aapo, Konsta
- Python
   - Aleksei, Ruslan
- Blazor
   - Aaro, Arttu, Lassi
- HTML, PHP
   - Waltteri, Okko, Eino
- Forms
   - Roope, Jimi, Alexi
- WPF
   - Luka, Aava, Riku
