using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject Formulario;
    public GameObject Avatares;
    public GameObject Agregar;
    private bool isOpenAgregar = true;
    private bool isOpenF = false;
    private bool isOpenA = false;
    string nombre = null;
    string descripcion = null;
    string clave = "";
    string claveCorrecta = "Marco1";
    public Transform Ubicacion;

    void Start()
    {

        

    }

    public void MostrarFormulario()
    {
        if (!isOpenF)
        {
            Formulario.SetActive(true);
            isOpenF=true;
            Agregar.SetActive(false);
            isOpenAgregar = false;
        }
        else
        {
            Formulario.SetActive(false);
            isOpenF=false;
            Agregar.SetActive(true);
            isOpenAgregar = true;
        }
    }
    public void SalirFormulario()
    {
        if (isOpenF)
        {
            Formulario.SetActive(false);
            isOpenF = false;
            Agregar.SetActive(true);
            isOpenAgregar = true;
        }
    }
    public void MostrarAvatares()
    {
        if (!isOpenA)
        {
            Avatares.SetActive(true);
            isOpenA = true;
        }
        else
        {
            Avatares.SetActive(false);
            isOpenA = false;
        }
    }



    public void AvatarButton()
    {
        
        
        MostrarAvatares();
        
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null) {
            if (go.tag == "ElAvatar") {
                SingletonVars.Instance.avatarGlobal = go.gameObject.name;
            }
        }
    }

    public void GuardarAvatar() {
        nombre = Formulario.transform.GetChild(0).GetComponent<InputField>().text;
        descripcion = Formulario.transform.GetChild(1).GetComponent<InputField>().text;
        clave = Formulario.transform.GetChild(2).GetComponent<InputField>().text;
        if (clave == claveCorrecta)
        {
            GameObject npcGO = Instantiate(Resources.Load("Prefabs/" + SingletonVars.Instance.avatarGlobal, typeof(GameObject)), Ubicacion.position, Quaternion.identity) as GameObject;

            npcGO.GetComponent<DialogueTrigger>().AddSentence(nombre, descripcion);
            
            SalirFormulario();
            Formulario.transform.GetChild(0).GetComponent<InputField>().text = "";
            Formulario.transform.GetChild(1).GetComponent<InputField>().text = "";
            Formulario.transform.GetChild(2).GetComponent<InputField>().text = "";
        }
        else {
            Formulario.transform.GetChild(0).GetComponent<InputField>().text = "";
            Formulario.transform.GetChild(1).GetComponent<InputField>().text = "";
            Formulario.transform.GetChild(2).GetComponent<InputField>().text = "";
        }

    }

}
