
namespace ECSModel
{
    public static class PlayerFactory
    {
        public static Player Create(long id)
        {
            SerializationComponent serialize = Game.Scene.GetComponent<SerializationComponent>();
            Player player = ComponentFactory.CreateWithId<Player>(id);
            player.UnitId = id;
            PlayerAttributeCom attCom = player.AddComponent<PlayerAttributeCom>();
            PlayerBallCom ballCom = player.AddComponent<PlayerBallCom>();
            PlayerBarCom barCom = player.AddComponent<PlayerBarCom>();
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
            playerComponent.Add(player);
            playerComponent.MyPlayer = player;
            
            InitPlayerAttribute(serialize, ref attCom);
            return player;
        }

        static void InitPlayerAttribute(  SerializationComponent serialize  ,  ref PlayerAttributeCom attCom )
        {
            PlayerAttributeCom serializePlayerAttribute = serialize.GetSerializePlayerAttributeCom();
            float f = 1.0f;
            int digit = 3;
            
            if (serializePlayerAttribute != null)
            {
                attCom.Diamond = serializePlayerAttribute.Diamond;
                attCom.PlayerCurLevel = serializePlayerAttribute.PlayerCurLevel;
                f = serializePlayerAttribute.BigNumberFloat;
                digit= serializePlayerAttribute.BigNumberDigit;
                attCom.InitMoney(f,digit);
            }
            else
            {
                attCom.InitMoney(f,digit);
            }
        }
        
    }
    
}