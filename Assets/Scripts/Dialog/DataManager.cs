using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

//JobList를 LinkedList로 만든 이유는, LLM에게 생성받은 Job을 추가하기 쉽기 때문임.
//최초 로딩시 n번째 부터 로딩하는지 기능 추가 필요 (기존에 n번째 job까지 실행했는지 저장)
//게임내에 미래가 바뀌는 분기가 있다면, 로직 수정필요


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

        ReplaceDict["Narrator"] = "나레이터";
        ReplaceDict["Player"] = "주인공";
        ReplaceDict["Shadow"] = "???";

        EventHandler.AfterRegisterUIManager += DoJob;
    }

    private void OnDisable()
    {
        EventHandler.AfterRegisterUIManager -= DoJob;
    }

    /// <summary>
    /// CSV파일을 불러와 JobList에 등록합니다. 기존에 남은 Job은 삭제합니다.
    /// </summary>
    /// <param name="filePath"></param> 
    public void LoadDialogFromCSV(string filePath, bool resetJobList = false)
    {
        filePath = _dialogFilePath + filePath + _dialogFileExtention;

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다.");
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
                Debug.LogError($"CSV파일에 잘못된 형식이 있습니다. line: {i + 1}");
                continue;
            }

            //DB 구조가 바뀌면 수정해야할 곳
            int jobType = int.Parse(parts[0].Trim());
            string narrator = parts[1].Trim();
            int isTalking = int.Parse(parts[2].Trim());
            string dialog = parts[3].Trim().Replace('*', ',');

            TEST_EVENT_INT_LIST.Add(jobType);

            //Job 생성
            DialogJobBase newJob = CreateDialogJob(narrator, dialog, jobType);
            if (newJob != null)
            {
                _jobList.AddLast(newJob);
            }
            else
            {
                Debug.LogError($"CSV 파일에 잘못된 event 형식이 있거나 등록되지 않은 Job Class 입니다. line: {i}");
            }
        }
    }

    /// <summary>
    /// 입력받은 string을 파싱해 string[]으로 바꿉니다.
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
            if (match.Groups[1].Success) // 큰따옴표로 감싸진 데이터
            {
                fields.Add(match.Groups[1].Value);
            }
            else if (match.Groups[2].Success) // 일반 데이터
            {
                fields.Add(match.Groups[2].Value);
            }
        }

        return fields.ToArray();
    }

    /// <summary>
    /// DB의 event 항목에 따라 job을 생성합니다.
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
    /// 현재 인덱스의 Job을 실행합니다. 남은 Job의 갯수를 리턴합니다.
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
    /// 나레이터의 id와 이름을 등록합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public void RegisterNarratorName(string id, string name)
    {
        ReplaceDict[id] = name;
    }

    /// <summary>
    /// DialogJob event가 생성되면 제일 앞에 추가합니다.
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
