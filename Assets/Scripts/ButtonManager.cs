using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject Formulario;
    public GameObject Avatares;
    public GameObject Agregar;
    private bool isOpenAgregar = true;
    private bool isOpenF = false;
    private bool isOpenA = false;
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

    }
}
