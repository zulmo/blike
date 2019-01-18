using System;
using System.Collections.Generic;

public static class ApplicationModels
{
    private static Dictionary<Type, object> _models = new Dictionary<Type, object>();

	public static void Initialize()
    {
        RegisterModel<GameModel>(new GameModel());
    }

    public static void Deinitialize()
    {
        UnregisterModel<GameModel>();
    }

    public static void RegisterModel<TType>(object model)
    {
        _models.Add(typeof(TType), model);
    }

    public static TType GetModel<TType>()
    {
        return (TType)_models[typeof(TType)];
    }

    public static void UnregisterModel<TType>()
    {
        _models.Remove(typeof(TType));
    }
}
