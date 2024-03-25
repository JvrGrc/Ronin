using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] Text speaker;
    [SerializeField] Text text;
    [SerializeField] GameObject dialogPanel;
    
    int cont;
    List<string> textoex = new List<string>(); 

    public void StartDialog(string filePath)
    {
        //  CSV file path
        string csvFilePath = filePath;

        //  Call the function to red de CSV and load the dialogs
        LoadDialogsFromCSV(csvFilePath);

        dialogPanel.SetActive(true);
        cont = 0;
        ShowDialog();
    }

    private void ShowDialog()
    {
        string[] line = textoex[cont].Split(':');
        speaker.text = line[0];
        if (line[1].StartsWith(" ")) line[1] = line[1].Remove(0, 1);
        if (line[1].StartsWith("*") && line[1].EndsWith("*"))
        {
            line[1] = line[1].Replace('*', ' ');
            text.text = line[1];
            text.fontStyle = FontStyle.Bold;
        }
        else
        {
            text.text = line[1];
            text.fontStyle = FontStyle.Normal;
        }
    }

    public void ContinueDialog()
    {
        cont++;
        if (cont > textoex.Count - 1)
        {
            dialogPanel.SetActive(false);
        }
        else
        {
            ShowDialog();
        }
    }

    private void LoadDialogsFromCSV(string filePath)
    {
        // Verify if the file exists
        if (File.Exists(filePath))
        {
            // Read all the lines of the file
            string[] lines = File.ReadAllLines(filePath);

            // Remove all the strings of the list before adding the new ones.
            textoex.Clear();

            for (int i = 0; i < lines.Length; i++)
            {
                // Add the string to the list
                textoex.Add(lines[i]);
            }
        }
        else
        {
            Debug.LogError("El archivo CSV no existe en la ruta especificada.");
        }
    }

    public void ReadFile(TextAsset text, int startLine, int endLine)
    {
        StringReader reader = new StringReader(text.text);
        List<string> lines = new List<string>();
        while (true)
        {
            string line = reader.ReadLine();

            if (line == null) break;
            else lines.Add(line);

            textoex.Clear();
        }
        // Verifies that the given numbers are in the file lenght range.
        startLine = Mathf.Clamp(startLine, 0, lines.Count - 1);
        endLine = Mathf.Clamp(endLine, 0, lines.Count - 1);

        for (int i = startLine; i <= endLine; i++)
        {
            textoex.Add(lines[i]);
        }
    }

    public void StartDialogFromTo(string filePath, int startLine, int endLine)
    {
        string csvFilePath = filePath;

        LoadDialogsFromCSV(csvFilePath, startLine, endLine);

        dialogPanel.SetActive(true);
        cont = 0;
        ShowDialog();
    }
    public void StartDialogFromTo(TextAsset text, int startLine, int endLine)
    {
        ReadFile(text, startLine, endLine);
        
        dialogPanel.SetActive(true);
        cont = 0;
        ShowDialog();
    }

    private void LoadDialogsFromCSV(string filePath, int startLine, int endLine)
    {
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath, Encoding.UTF8);
            string line;
            List<string> linesRead = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                // Procesa cada línea del archivo
                linesRead.Add(line);
            }
            //string[] lines = File.ReadAllLines(filePath);

            textoex.Clear();

            // Verifies that the given numbers are in the file lenght range.
            startLine = Mathf.Clamp(startLine, 0, linesRead.Count - 1);
            endLine = Mathf.Clamp(endLine, 0, linesRead.Count - 1);

            for (int i = startLine; i <= endLine; i++)
            {
                textoex.Add(linesRead[i]);
            }
        }
        else
        {
            Debug.LogError("El archivo CSV no existe en la ruta especificada.");
        }
    }
}
