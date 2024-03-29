using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

interface IDictionnaire
{
    bool ContientMot(string mot);
    string TrouverCorrespondance(string mot);
}

class Dictionnaire : IDictionnaire
{
    private List<string> mots;

    public Dictionnaire(string cheminFichier)
    {
        mots = File.ReadAllLines(cheminFichier).ToList();
    }

    public bool ContientMot(string mot)
    {
        return mots.Contains(mot);
    }

    public string TrouverCorrespondance(string mot)
    {
        string lettreTrie = new string(mot.OrderBy(c => c).ToArray());
        return mots.FirstOrDefault(m => new string(m.OrderBy(c => c).ToArray()) == lettreTrie);
    }
}

class MoteurRecherche
{
    private IDictionnaire dictionnaire;

    public MoteurRecherche(IDictionnaire dictionnaire)
    {
        this.dictionnaire = dictionnaire;
    }

    public void RechercherMots(string[] mots)
    {
        foreach (string mot in mots)
        {
            string motTraite = mot.Trim();

            if (dictionnaire.ContientMot(motTraite))
            {
                AfficherCorrespondance(motTraite, motTraite);
            }
            else
            {
                string correspondance = dictionnaire.TrouverCorrespondance(motTraite);

                if (correspondance != null)
                {
                    AfficherCorrespondance(motTraite, correspondance);
                }
                else
                {
                    Console.WriteLine($"{motTraite} aucune correspondance trouvée");
                }
            }
        }
    }

    private void AfficherCorrespondance(string mot, string correspondance)
    {
        Console.WriteLine($"{mot} correspond à {correspondance}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        string dictionnairePath = "dictionnaire.txt";
        string[] motsUtilisateur;

        IDictionnaire dictionnaire = new Dictionnaire(dictionnairePath);
        MoteurRecherche moteurRecherche = new MoteurRecherche(dictionnaire);

        Console.WriteLine("Entrez les mots séparés par des virgules :");
        string entreeUtilisateur = Console.ReadLine();

        motsUtilisateur = entreeUtilisateur.Split(',');

        moteurRecherche.RechercherMots(motsUtilisateur);

        Console.ReadKey();
    }
}
