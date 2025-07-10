using UnityEngine;

// Este script gestiona si el jugador lleva ropa, y si está limpia o sucia
public class PlayerLaundry : MonoBehaviour
{
    public bool carryingLaundry = false;  // Lleva ropa
    public bool laundrySoiled = false;    // Está sucia

    public GameObject cleanIcon; // Icono visible si ropa está limpia
    public GameObject dirtyIcon; // Icono visible si ropa está sucia

    // Llamado al recoger ropa limpia de la lavadora
    public void PickupLaundry()
    {
        carryingLaundry = true;
        laundrySoiled = false;
        cleanIcon.SetActive(true);   // Mostrar icono de limpia
        dirtyIcon.SetActive(false);  // Ocultar icono sucia
    }

    // Llamado si una caca golpea al jugador
    public void SoilLaundry()
    {
        if (carryingLaundry)
        {
            laundrySoiled = true;
            cleanIcon.SetActive(false);  // Ocultar icono de limpia
            dirtyIcon.SetActive(true);   // Mostrar icono sucia
            Debug.Log("¡Te ensuciaste la ropa! Vuelve a lavarla.");
        }
    }

    // Llamado al tender ropa correctamente
    public void DeliverLaundry()
    {
        carryingLaundry = false;
        cleanIcon.SetActive(false);
        dirtyIcon.SetActive(false);
        Debug.Log("Ropa tendida correctamente.");
    }
}
