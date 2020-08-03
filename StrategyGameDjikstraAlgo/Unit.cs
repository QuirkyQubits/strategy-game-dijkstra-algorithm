using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public enum UnitTypes {
        Soldier,
        Seaman
    }

    public class Unit
    {
        public UnitTypes unitType;
        public int movementPoints;
        public Stats stats;
        public Coordinate currentLocation;
        public string name;

        private Unit[,] board;

        public Unit(
            Unit[,] board,
            UnitTypes unitType,
            int movementPoints,
            string name,
            Stats stats)
        {
            this.board = board;
            this.unitType = unitType;
            this.movementPoints = movementPoints;
            this.stats = stats;
            this.name = name;
        }

        public bool Dead()
        {
            return stats.hp <= 0;
        }

        public void TakeTurn()
        {
            if (!Dead())
            {
                Move();
                Attack();
            }
        }

        //moves the unit around the gameBoard
        private void Move() {

            checkUnitIsNotDead();

            Coordinate target = GetUserInputForMovement();

            // move there
            if (!target.Equals(currentLocation))
            {
                board[currentLocation.r, currentLocation.c] = null;
                currentLocation = target;
                board[currentLocation.r, currentLocation.c] = this;
            }
        }

        private void Attack() {

            checkUnitIsNotDead();

            Coordinate targetCoord = GetUserInputForAttack();

            Unit unitToAttack = board[targetCoord.r, targetCoord.c];

            checkTargetToAttackIsNotNull(unitToAttack);

            unitToAttack.stats.hp -= stats.atk;
        }

        private void checkUnitIsNotDead()
        {
            bool unitIsDead = stats.hp <= 0;
            string errorMessage = "Unit.Attack():: selected unit is already dead";
            Debug.Assert(!unitIsDead, errorMessage);
            if (unitIsDead)
                throw new GameException(errorMessage);

        }

        private void checkTargetToAttackIsNotNull(Unit unit)
        {
            string errorMessage = "Unit.Attack():: target to attack is null";
            Debug.Assert(unit != null, errorMessage);
            if (unit == null)
                throw new GameException(errorMessage);
        }

        private Coordinate GetUserInputForMovement()
        {
            // <code>

            // call djikstra's algo
            // then let the player choose among the options



            // check the coord the player chose
            // is in the hashmap returned by Djikstra
            // before returning

            // comment this out after implementing
            return new Coordinate(0, 0); 
        }

        private Coordinate GetUserInputForAttack()
        {
            // <code>

            // show coordinates of any attackable enemies
            // (adjacent for now)
            // then let the player choose among the options



            // check the coord the player chose is valid before returning



            // comment this out after implementing
            return new Coordinate(0, 0);
        }
    }
}
