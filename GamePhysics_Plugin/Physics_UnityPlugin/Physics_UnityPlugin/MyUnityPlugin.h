#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H
#include "lib.h"

#ifdef __cplusplus
extern "C"
{
#else //!__cplusplus

#endif // __cplusplus

MYUNITYPLUGIN_SYMBOL int InitFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int DoFoo(int bar);
MYUNITYPLUGIN_SYMBOL int TermFoo();
MYUNITYPLUGIN_SYMBOL void TestDebugCalls();


#ifdef __cplusplus
}
#else //!__cplusplus

#endif // __cplusplus


#endif // !MYUNITYPLUGIN_H
 
