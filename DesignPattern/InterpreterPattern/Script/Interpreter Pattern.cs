using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpreterPattern 
{
    // ���� ��Ģ�� �������� ����
    /*
          IF player_health < 10 THEN activate_healing_ability
     */

    // Context Ŭ����: �ؼ��� ������ ������ �����ϴ� Ŭ����
    public class Context
    {
        public Dictionary<string, int> Variables { get; private set; }

        public Context()
        {
            Variables = new Dictionary<string, int>();
        }
    }

    // AbstractExpression Ŭ����: ���� ��Ģ�� ��Ÿ���� �߻� Ŭ����
    public abstract class AbstractExpression
    {
        public abstract bool Interpret(Context context);
    }

    // TerminalExpression Ŭ����: ���� ������ �ؼ��ϴ� Ŭ����
    public class TerminalExpression : AbstractExpression
    {
        private string variable;
        private int value;

        public TerminalExpression(string variable, int value)
        {
            this.variable = variable;
            this.value = value;
        }

        public override bool Interpret(Context context)
        {
            if (context.Variables.ContainsKey(variable))
            {
                int variableValue = context.Variables[variable];
                return variableValue < value;
            }
            return false;
        }
    }

    // NonTerminalExpression Ŭ����: ���� ������ �ؼ��ϴ� Ŭ����
    public class NonTerminalExpression : AbstractExpression
    {
        private AbstractExpression expression1;
        private AbstractExpression expression2;

        public NonTerminalExpression(AbstractExpression expression1, AbstractExpression expression2)
        {
            this.expression1 = expression1;
            this.expression2 = expression2;
        }

        public override bool Interpret(Context context)
        {
            return expression1.Interpret(context) && expression2.Interpret(context);
        }
    }

    // Interpreter Ŭ����: DSL ������ �ؼ��ϴ� Ŭ����
    public class Interpreter
    {
        private AbstractExpression expression;

        public Interpreter(AbstractExpression expression)
        {
            this.expression = expression;
        }

        public bool Interpret(Context context)
        {
            return expression.Interpret(context);
        }
    }

    // Ŭ���̾�Ʈ �ڵ�: Interpreter ������ ����ϴ� ����
    public class Client
    {
        public void Run()
        {
            // DSL�� �ؼ��� ���� ����
            AbstractExpression expression = new NonTerminalExpression(
                new TerminalExpression("player_health", 10),
                new TerminalExpression("enemy_health", 5)
            );

            // ������ �ؼ��� ���ؽ�Ʈ ����
            Context context = new Context();
            context.Variables["player_health"] = 8;
            context.Variables["enemy_health"] = 7;

            // ���� �ؼ� �� ����
            Interpreter interpreter = new Interpreter(expression);
            bool ruleResult = interpreter.Interpret(context);

            Debug.Log("���� �� ���: " + ruleResult); // ���: False
        }
    }
}
