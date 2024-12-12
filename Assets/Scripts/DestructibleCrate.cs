using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
   public static event EventHandler OnAnyCrateDestroyed;
   private GridPosition _gridPosition;
   
   [SerializeField] private Transform crateDestroyed;
   
   private void Start()
   {
      _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
   }
   
   public void Damage()
   {  
      OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
      Instantiate(crateDestroyed, transform.position, Quaternion.identity);
      Destroy(gameObject);
   }

   public GridPosition GetGridPosition()
   {
      return _gridPosition;
   }
}
