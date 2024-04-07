using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string _toneSheet="**--****----***----";
    private string toneSheet="";
    public int health=10;
    public int currentNote;
    private int subCurrentNote;
    public TextMeshProUGUI tune;
    public TextMeshProUGUI hpvisual;
    public int maxVisual;
    private LayerMask groudMask;

    private void Start()   
    {
       
        currentNote = _toneSheet.Length-1;
        subCurrentNote = maxVisual;
        toneSheet= _toneSheet.Substring(_toneSheet.Length-1-maxVisual,maxVisual);
        tune.text = toneSheet;
        hpvisual.text = health.ToString();
    }

    public void TakeDame(HitType hitType,Vector3 source)
    {

        Ray ray = new Ray(transform.position, Vector3.down);
        
         transform.DOPunchPosition((transform.position - source).normalized, 0.4f, 20, 11).SetEase(Ease.Linear);

        if (IsValidNote(hitType)&&currentNote>0)
        {
          
                VisualTune();
        }
        else
        {
            health -= 1;
            hpvisual.text=health.ToString();    
        }
        DeathHandler(source);
    }

    private void DeathHandler(Vector3 source)
    {
        if (health <= 0)
        {
            Vector3 normalized = (source - transform.position).normalized;

            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.AddForce(normalized * 1000, ForceMode.Impulse);
            rigid.constraints = RigidbodyConstraints.None;
            Destroy(gameObject, 3);
        }
    }

    private void VisualTune()
    {
        currentNote -= 1;

        if (currentNote < 0)
        {
            currentNote = 0;
            Debug.Log("stun");
        }



        toneSheet = toneSheet.Substring(0, subCurrentNote - 1);
        subCurrentNote--;
        tune.text = toneSheet;
        if (subCurrentNote == 0)
        {
            subCurrentNote = maxVisual;
            int startnote = currentNote - maxVisual;
            toneSheet = Helper.CreateSubstring(_toneSheet, startnote <= 0 ? 0 : startnote, Math.Min(currentNote, maxVisual));
        }
        tune.text = toneSheet;
    }

    private bool IsValidNote(HitType hitType)
    {
        if(currentNote<=0) return true;
        int v = currentNote - 1;
        switch (_toneSheet[v<0?0:v])
        {
            case '*':
                if (hitType == HitType.Tap)
                return true;
                break;
            case '-': 
                if(hitType==HitType.Hold)
                return true;
                break;
           
        }
       
        return false;
    }
}
