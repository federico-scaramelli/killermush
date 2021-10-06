using UnityEngine;
using System.Collections;

/// <summary>
/// Classe utile a gestire l'equipaggiamento delle armi da parte del player.
/// </summary>
public class WeaponManagerTest : MonoBehaviour
{
	//public Weapon[] weapons;
	public WeaponTest startWeapon;
	public WeaponTest currentWeapon;
	public Transform weaponPoint; //Punto in cui istanzierò l'oggetto dell'arma corrente. (Grafica)

	public void Start()
	{
		ChangeWeapon(startWeapon);
	}

	/// <summary>
	/// Funzione utile a far equipaggiare una nuova arma al player.
	/// </summary>
	/// <param name="weapon">Arma da equipaggiare</param>
	public void ChangeWeapon(WeaponTest weapon)
	{
		//for (int i = 0; i < weapons.Length; i++)
		//{
		//    weapons[i].transform.gameObject.SetActive(false);
		//}
		//weapon.transform.gameObject.SetActive(true);

		if (currentWeapon != null)
		{
			foreach(GameObject bullet in currentWeapon.bullets)
			{
				Destroy(bullet);
			}
			Destroy(currentWeapon.gameObject);
		}

		//Assegno a currentWeapon l'oggetto dell'arma che istanzio
		currentWeapon = (WeaponTest)Instantiate(weapon, weaponPoint.position, transform.rotation);
		currentWeapon.transform.SetParent(transform); //Imposto il player come parent dell'arma corrente 
		//currentWeapon = weapon;
		currentWeapon.firstShot = true; //Dato che ho appena cambiato arma sarà il primo sparo

		//Avendo come gerarchia delle armi uno schema fisso, prelevo i vari componenti
		currentWeapon.shootPoint = currentWeapon.transform.GetChild(0).gameObject;
		currentWeapon.gunLine = currentWeapon.shootPoint.transform.GetComponent<LineRenderer>();
		currentWeapon.gunLight = currentWeapon.shootPoint.transform.GetComponent<Light>();
		currentWeapon.faceLight = currentWeapon.shootPoint.transform.GetChild(1).GetComponent<Light>();
		currentWeapon.muzzleFlash = currentWeapon.shootPoint.transform.GetChild(0).GetComponent<ParticleSystem>();
	}

	//Funzione di prova non utilizzata
	public void ChangeActiveWeapon(WeaponTest weapon)
	{
		currentWeapon = weapon;
		currentWeapon.firstShot = true;

		currentWeapon.shootPoint = currentWeapon.transform.GetChild(0).gameObject;
		currentWeapon.gunLine = currentWeapon.shootPoint.transform.GetComponent<LineRenderer>();
		currentWeapon.gunLight = currentWeapon.shootPoint.transform.GetComponent<Light>();
		currentWeapon.faceLight = currentWeapon.shootPoint.transform.GetChild(1).GetComponent<Light>();
		currentWeapon.muzzleFlash = currentWeapon.shootPoint.transform.GetChild(0).GetComponent<ParticleSystem>();
	}
}
