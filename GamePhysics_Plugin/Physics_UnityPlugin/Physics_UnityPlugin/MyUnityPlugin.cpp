#include "MyUnityPlugin.h"
#include "foo.h"
#include "DebugCPP.h"

Foo* inst = 0;

int InitFoo(int f_new)
{
	if (!inst)
	{
		inst = new Foo(f_new);
		return 1;
	}

	return 0;
}

int DoFoo(int bar)
{
	if (inst)
	{
		int result = inst->foo(bar);
		return result;
	}

	return 0;
}

int TermFoo()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}
	return 0;
}

void __stdcall TestDebugCalls()
{
	Debug::Log("Test!", Color::Red);
	Debug::Log("Test!", Color::Green);
	Debug::Log("Test!", Color::White);
	Debug::Log("Test!", Color::Orange);
	Debug::Log("Test!", Color::Yellow);
	Debug::Log("Test!", Color::Black);
	Debug::Log("Test!", Color::Blue);

	Debug::Log(true, Color::Black);
	Debug::Log(false, Color::Red);

}