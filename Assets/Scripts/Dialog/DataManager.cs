using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

//JobList�� LinkedList�� ���� ������, LLM���� �������� Job�� �߰��ϱ� ���� ������.
//���� �ε��� n��° ���� �ε��ϴ��� ��� �߰� �ʿ� (������ n��° job���� �����ߴ��� ����)
//���ӳ��� �̷��� �ٲ�� �бⰡ �ִٸ�, ���� �����ʿ�


public class DataManager : SingletonMonobehavior<DataManager>
{
    private string _dialogFilePath = "Assets/Data/";
    private string _dialogFileExtention = ".csv";

    private LinkedList<DialogJobBase> _jobList = new LinkedList<DialogJobBase>();
    private DialogJobBase _currentJob = null;

    public Dictionary<string, string> ReplaceDict = new Dictionary<string, string>();
    public List<int> TEST_EVENT_INT_LIST = new List<int>();

    protected override void Awake()
    {
        base.Awake();

        ReplaceDict["Narrator"] = "��������";
        ReplaceDict["Player"] = "���ΰ�";
        ReplaceDict["Shadow"] = "???";

        EventHandler.AfterRegisterUIManager += DoJob;
    }

    private void OnDisable()
    {
        EventHandler.AfterRegisterUIManager -= DoJob;
    }

    /// <summary>
    /// CSV������ �ҷ��� JobList�� ����մϴ�. ������ ���� Job�� �����մϴ�.
    /// </summary>
    /// <param name="filePath"></param> 
    public void LoadDialogFromCSV(string filePath, bool resetJobList = false)
    {
        filePath = _dialogFilePath + filePath + _dialogFileExtention;

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV ������ �������� �ʽ��ϴ�.");
            return;
        }

        if(resetJobList)
        {
            _jobList.Clear();
        }
        //??
        _currentJob = null;

        string[] lineArray = File.ReadAllLines(filePath);
        for(int i = 1; i < lineArray.Length; i++)
        { 
            string line = lineArray[i];

            string[] parts = ParseCsvLine(line);
            if(parts.Length < 4)
            {
                Debug.LogError($"CSV���Ͽ� �߸��� ������ �ֽ��ϴ�. line: {i + 1}");
                continue;
            }

            //DB ������ �ٲ�� �����ؾ��� ��
            int jobType = int.Parse(parts[0].Trim());
            string narrator = parts[1].Trim();
            int isTalking = int.Parse(parts[2].Trim());
            string dialog = parts[3].Trim().Replace('*', ',');

            TEST_EVENT_INT_LIST.Add(jobType);

            //Job ����
            DialogJobBase newJob = CreateDialogJob(narrator, dialog, jobType);
            if (newJob != null)
            {
                _jobList.AddLast(newJob);
            }
            else
            {
                Debug.LogError($"CSV ���Ͽ� �߸��� event ������ �ְų� ��ϵ��� ���� Job Class �Դϴ�. line: {i}");
            }
        }
    }

    /// <summary>
    /// �Է¹��� string�� �Ľ��� string[]���� �ٲߴϴ�.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string[] ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();
        string pattern = @"""([^""]*)""|([^,]+)";
        Regex regex = new Regex(pattern);

        foreach (Match match in regex.Matches(line))
        {
            if (match.Groups[1].Success) // ū����ǥ�� ������ ������
            {
                fields.Add(match.Groups[1].Value);
            }
            else if (match.Groups[2].Success) // �Ϲ� ������
            {
                fields.Add(match.Groups[2].Value);
            }
        }

        return fields.ToArray();
    }

    /// <summary>
    /// DB�� event �׸� ���� job�� �����մϴ�.
    /// </summary>
    /// <param name="narrator"></param>
    /// <param name="dialog"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private DialogJobBase CreateDialogJob(string narrator, string dialog, int type)
    {
        switch (type)
        {
            case 100:
                return new DialogJob(narrator, dialog);
            case 200:
                return new InputJob_NarratorName(narrator, dialog);
            case 250:
                return new InputJob_LLM(narrator, dialog);
            case 300:
                return new SelectJob(narrator, dialog);
        }
        return null;
    }

    /// <summary>
    /// ���� �ε����� Job�� �����մϴ�. ���� Job�� ������ �����մϴ�.
    /// </summary>
    public int DoNextJob()
    {
        int jobCount = _jobList.Count;

        DoJob();

        return jobCount;
    }


    private void DoJob()
    {
        if (_currentJob != null)
        {
            if (!_currentJob.IsFinish)
            {
                return;
            }
            else
            {
                _currentJob.AfterFinish();
            }
        }

        if (_jobList.Count > 0)
        {
            _currentJob = _jobList.First.Value;
            _jobList.RemoveFirst();
            _currentJob.Excute();
        }
    }

    /// <summary>
    /// ���������� id�� �̸��� ����մϴ�.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public void RegisterNarratorName(string id, string name)
    {
        ReplaceDict[id] = name;
    }

    /// <summary>
    /// DialogJob event�� �����Ǹ� ���� �տ� �߰��մϴ�.
    /// </summary>
    /// <param name="job"></param>
    public void RegeisterDialogJob(DialogJobBase job)
    {
        if(job!=null)
        {
            _jobList.AddFirst(job);
        }
    }
}
