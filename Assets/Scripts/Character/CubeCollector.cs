using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollector : MonoBehaviour
{
    [SerializeField] Transform _parent;

    [SerializeField] GameObject _character;

    [SerializeField] GameObject _cubePrefab;

    [SerializeField] GameObject _firstCube;

    [SerializeField] CollectedCubes _collectedCubes;

    private float _currentHeight = 0;

    private float _heightOffset = 1f;

    private CollectableCubes _collectableCubes;

    private int _countCollectedCubes;

    private void Awake()
    {
        _collectedCubes.cubes.Add(_firstCube);
    }

    /// <summary>
    /// reduce overall height when destroying cubes
    /// </summary>
    public void OnDestroyCube()
    {
        _currentHeight -= _heightOffset;
    }

    /// <summary>
    /// create cubes and move them under the character
    /// </summary>
    public void InstantiateCubes(int countCollectedCubes)
    {
        _countCollectedCubes = countCollectedCubes;
        
        for (int i = 0; i < _countCollectedCubes; i++)
        {
            Vector3 newPos = new Vector3(0, 1, 0);
            _firstCube.transform.position += newPos;
            _character.transform.position += newPos;

            GameObject newCube = Instantiate(_cubePrefab, transform.position + new Vector3(0, _currentHeight, 0), Quaternion.identity);
            newCube.transform.SetParent(_parent);
            Debug.Log(_currentHeight);
            _currentHeight += _heightOffset;
            _collectedCubes.cubes.Add(newCube);
        }
    }
}
