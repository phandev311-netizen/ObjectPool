# Object Pool System for Unity

A lightweight and reusable **Object Pool System** for Unity that minimizes runtime allocations by reusing `GameObject` instances instead of repeatedly calling `Instantiate()` and `Destroy()`.

This implementation is simple, easy to integrate into existing projects, and works well for frequently spawned objects such as:

- Bullets
- Enemies
- Particle Effects
- Explosions
- Item Drops
- UI Elements
- Projectiles

---

# Features

- ✅ Generic Object Pool
- ✅ Automatic pool creation
- ✅ Singleton Pool Manager
- ✅ Supports multiple prefab types
- ✅ Automatic pool expansion
- ✅ Component-based retrieval
- ✅ Simple API
- ✅ Easy integration into any Unity project

---

# Folder Structure

```
Scripts/
│
├── ObjectPool.cs
└── PoolManager.cs
```

---

# System Architecture

```
                    PoolManager
                         │
      ┌──────────────────┼──────────────────┐
      │                  │                  │
      ▼                  ▼                  ▼
 Enemy Pool         Bullet Pool       Effect Pool
      │                  │                  │
 ┌────┴────┐        ┌────┴────┐       ┌─────┴─────┐
 │         │        │         │       │           │
Enemy1   Enemy2  Bullet1  Bullet2  Effect1  Effect2
```

Each prefab owns an independent **ObjectPool**.

The **PoolManager** stores and manages every pool.

---

# Why Object Pooling?

Creating and destroying GameObjects repeatedly is expensive.

Instead of:

```csharp
Instantiate(prefab);

...

Destroy(gameObject);
```

the Object Pool follows this workflow:

```
Create once
      │
      ▼
Disable
      │
      ▼
Reuse
      │
      ▼
Disable again
```

Benefits include:

- Reduced Garbage Collection (GC)
- Fewer CPU spikes
- Improved frame rate
- Better runtime performance
- Smoother gameplay

---

# Components

## ObjectPool

The `ObjectPool` class manages a single prefab type.

Example:

```
Enemy Prefab
        │
        ▼
 Enemy Object Pool
```

Another prefab will have its own pool.

```
Bullet Prefab
        │
        ▼
 Bullet Object Pool
```

---

## Fields

### Prefab

```csharp
private GameObject _prefab;
```

The original prefab used to create pooled objects.

---

### Objects

```csharp
private List<GameObject> _objects;
```

Stores every object that belongs to this pool.

Example:

```
Enemy(Clone)
Enemy(Clone)
Enemy(Clone)
Enemy(Clone)
```

---

### Parent

```csharp
private Transform _parent;
```

The parent transform that keeps the hierarchy organized.

Hierarchy example:

```
PoolManager
    Enemy(Clone)
    Enemy(Clone)
    Bullet(Clone)
```

---

# Constructor

```csharp
public ObjectPool(
    GameObject prefab,
    int initAmount,
    Transform parent)
```

When a pool is created, it will:

1. Store the prefab.
2. Store the parent transform.
3. Pre-create a number of objects.
4. Disable every object.

Example:

```
Initial Amount = 10

↓

Enemy1
Enemy2
Enemy3
...
Enemy10
```

All objects start as inactive.

---

# CreateInstance()

```csharp
private GameObject CreateInstance()
```

Responsible for creating a new pooled object.

Workflow:

```
Instantiate(prefab)
        │
        ▼
Set Parent
        │
        ▼
SetActive(false)
        │
        ▼
Store in List
```

This is the only function that creates new GameObjects.

---

# GetObject<T>()

```csharp
public T GetObject<T>()
```

Returns a component of type `T`.

Workflow:

```
Search the pool

        │

Inactive object found?

   Yes              No
    │               │
    ▼               ▼
Enable         Create Instance
    │               │
    └───────┬───────┘
            ▼
Return Component
```

Example:

```csharp
Enemy enemy =
Pool.GetObject<Enemy>();
```

---

# GetObject()

```csharp
public GameObject GetObject()
```

Returns the entire GameObject.

Example:

```csharp
GameObject bullet =
Pool.GetObject();
```

---

# PoolManager

`PoolManager` is a Singleton responsible for managing all pools.

Instead of creating pools manually, simply use:

```csharp
PoolManager.Instance
```

---

# Singleton

```csharp
public static PoolManager Instance;
```

Initialization:

```csharp
if (Instance == null)
    Instance = this;
else
    Destroy(gameObject);
```

Ensures that only one PoolManager exists.

---

# Initial Pool Size

```csharp
[SerializeField]
private int _initAmount = 10;
```

Number of objects created initially for every prefab.

Example:

```
Enemy

↓

10 Enemy Objects
```

---

# Preloaded Prefabs

```csharp
[SerializeField]
private List<GameObject> _preloadPrefabs;
```

Prefabs that should have pools created during startup.

Example:

```
Enemy
Bullet
Explosion
Coin
```

---

# Pool Dictionary

```csharp
Dictionary<GameObject, ObjectPool>
```

Stores the relationship between prefabs and pools.

```
Prefab
    │
    ▼
ObjectPool
```

Example:

```
Enemy Prefab

↓

Enemy Pool
```

---

# Initialization

When the scene starts:

```
PoolManager Awake

        │

Loop through all prefabs

        │

Pool already exists?

        │

No

        │

Create ObjectPool

        │

Store inside Dictionary
```

---

# Lazy Pool Creation

If a prefab wasn't preloaded:

```
Request Pool

        │

Pool exists?

     Yes       No
      │         │
      ▼         ▼
 Return     Create Pool
```

The pool is automatically generated when first requested.

---

# Usage

## Setup

1. Create an empty GameObject.

```
PoolManager
```

2. Attach:

```
PoolManager.cs
```

3. Assign prefabs inside the Inspector.

```
Enemy
Bullet
Explosion
Coin
```

---

# Spawn Objects

Retrieve a component:

```csharp
Enemy enemy =
PoolManager.Instance.GetObject<Enemy>(enemyPrefab);

enemy.transform.position = spawnPoint.position;
```

Or retrieve the GameObject:

```csharp
GameObject enemy =
PoolManager.Instance.GetObject(enemyPrefab);

enemy.transform.position = spawnPoint.position;
```

---

# Return Objects

When finished using an object:

```csharp
gameObject.SetActive(false);
```

The next request will reuse the same object.

---

# Complete Workflow

```
                Game Starts
                     │
                     ▼
          PoolManager Awake()
                     │
                     ▼
      Create pools for each prefab
                     │
                     ▼
      Create initial pooled objects
                     │
                     ▼
          SetActive(false)
                     │
──────────────────────────────────────
                     │
              Gameplay requests
                     │
                     ▼
      PoolManager.GetObject(prefab)
                     │
                     ▼
        Search inactive object
             │              │
            Yes            No
             │              │
             ▼              ▼
      Enable Object   Create New Object
             │              │
             └──────┬───────┘
                    ▼
              Return Object
                    │
                    ▼
             Gameplay Logic
                    │
                    ▼
         SetActive(false)
                    │
                    ▼
           Back to the Pool
```

---

# Example

### Spawn Bullet

```csharp
Bullet bullet =
PoolManager.Instance.GetObject<Bullet>(bulletPrefab);

bullet.transform.position = firePoint.position;
bullet.transform.rotation = firePoint.rotation;
```

---

### Spawn Explosion

```csharp
GameObject explosion =
PoolManager.Instance.GetObject(explosionPrefab);

explosion.transform.position = hitPoint;
```

---

### Return Object

```csharp
gameObject.SetActive(false);
```

---

# Advantages

- Simple implementation
- Easy to understand
- Lightweight
- Automatic pool creation
- Automatic pool expansion
- Reduces `Instantiate()` calls
- Eliminates unnecessary `Destroy()` calls
- Reduces Garbage Collection
- Improves runtime performance
- Suitable for most Unity projects

---

# Limitations

The current implementation does **not** include:

- Explicit `Release(GameObject)` API
- Maximum pool size
- Automatic object state reset
- Pool statistics
- Addressables support
- Resources loading
- Asynchronous loading
- Pool shrinking

---

# Possible Improvements

Future enhancements could include:

- `IPoolable` interface
- `OnSpawn()`
- `OnDespawn()`
- Automatic reset callbacks
- Maximum pool capacity
- Dynamic pool resizing
- Object prewarming per prefab
- Pool monitoring/debugging
- Addressables integration
- Generic pooling for non-GameObjects

---

# Performance

Without Object Pooling:

```
Instantiate()
↓

Destroy()

↓

Instantiate()

↓

Destroy()
```

With Object Pooling:

```
Instantiate Once

↓

Reuse

↓

Reuse

↓

Reuse

↓

Reuse
```

This significantly reduces runtime memory allocations and CPU overhead during gameplay.

---

# License

This project is free to use, modify, and integrate into personal or commercial Unity projects.
