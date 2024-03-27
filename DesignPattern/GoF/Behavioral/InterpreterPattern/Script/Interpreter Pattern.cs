using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpreterPattern 
{
    // 게임 규칙을 동적으로 정의
    /*
          IF player_health < 10 THEN activate_healing_ability
     */

    // Context 클래스: 해석할 문장의 정보를 유지하는 클래스
    public class Context
    {
        public Dictionary<string, int> Variables { get; private set; }

        public Context()
        {
            Variables = new Dictionary<string, int>();
        }
    }

    // AbstractExpression 클래스: 문법 규칙을 나타내는 추상 클래스
    public abstract class AbstractExpression
    {
        public abstract bool Interpret(Context context);
    }

    // TerminalExpression 클래스: 단일 조건을 해석하는 클래스
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

    // NonTerminalExpression 클래스: 여러 조건을 해석하는 클래스
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

    // Interpreter 클래스: DSL 문장을 해석하는 클래스
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

    // 클라이언트 코드: Interpreter 패턴을 사용하는 예시
    public class Client
    {
        public void Run()
        {
            // DSL을 해석할 문장 생성
            AbstractExpression expression = new NonTerminalExpression(
                new TerminalExpression("player_health", 10),
                new TerminalExpression("enemy_health", 5)
            );

            // 문장을 해석할 컨텍스트 생성
            Context context = new Context();
            context.Variables["player_health"] = 8;
            context.Variables["enemy_health"] = 7;

            // 문장 해석 및 실행
            Interpreter interpreter = new Interpreter(expression);
            bool ruleResult = interpreter.Interpret(context);

            Debug.Log("게임 룰 결과: " + ruleResult); // 출력: False
        }
    }
}
