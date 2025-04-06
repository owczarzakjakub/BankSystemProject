using System;

public class Operacje_BazyDanych
{
    private string sciezkaPliku;

    public PlikTekstowy(string sciezkaPliku)
    {
        this.sciezkaPliku = sciezkaPliku;

        // Upewnij się, że plik istnieje
        if (!File.Exists(sciezkaPliku))
        {
            File.Create(sciezkaPliku).Close();
        }
    }

    // Dodawanie rekordu do pliku
    public void DodajRekord(string rekord)
    {
        File.AppendAllText(sciezkaPliku, rekord + Environment.NewLine);
    }

    // Znajdowanie rekordów zawierających konkretny tekst
    public List<string> ZnajdzRekordy(string tekstSzukany)
    {
        var rekordy = File.ReadAllLines(sciezkaPliku).ToList();
        return rekordy.Where(r => r.Contains(tekstSzukany, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Odczytywanie wszystkich rekordów z pliku
    public List<string> OdczytajRekordy()
    {
        return File.ReadAllLines(sciezkaPliku).ToList();
    }

    // Nadpisanie całego pliku nowymi danymi
    public void NadpiszRekordy(List<string> noweRekordy)
    {
        File.WriteAllLines(sciezkaPliku, noweRekordy);
    }

    // Usuwanie rekordu zawierającego konkretny tekst
    public void UsunRekord(string tekstDoUsuniecia)
    {
        var rekordy = File.ReadAllLines(sciezkaPliku).ToList();
        rekordy = rekordy.Where(r => !r.Contains(tekstDoUsuniecia)).ToList();
        File.WriteAllLines(sciezkaPliku, rekordy);
    }

    // Usuwanie rekordu po indeksie (np. 0 - pierwszy wiersz)
    public void UsunRekordPoIndeksie(int indeks)
    {
        var rekordy = File.ReadAllLines(sciezkaPliku).ToList();
        if (indeks >= 0 && indeks < rekordy.Count)
        {
            rekordy.RemoveAt(indeks);
            File.WriteAllLines(sciezkaPliku, rekordy);
        }
    }

    public string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    public bool VerifyPassword(string password, string hashedPassword)
    {
        var hashOfInput = HashPassword(password);
        if (hashOfInput == hashedPassword)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
