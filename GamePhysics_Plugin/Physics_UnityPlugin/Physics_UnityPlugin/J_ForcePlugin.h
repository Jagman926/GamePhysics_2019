#ifndef J_ForcePlugin_H
#define J_ForcePlugin_H

#include <math.h>

//https://stackoverflow.com/questions/53174216/update-vector3-array-from-c-native-plugin

struct J_Vector2
{
public:
	J_Vector2(float inX, float inY, float inMangitude)
	{
		x = inX;
		y = inY;
		mangitude = inMangitude;
	}

	J_Vector2(float inX, float inY)
	{
		x = inX;
		y = inY;
		calculateMagnitude();
	}

	J_Vector2()
	{

	}

	float x = 0.0f;
	float y = 0.0f;
	float mangitude = 0.0f;

	void calculateMagnitude()
	{
		mangitude = abs(sqrt(x * x + y * y));
	}

	J_Vector2 normalize() //Returns a normalized vector
	{
		J_Vector2 out(x, y);
		out.x = x / mangitude;
		out.y = y / mangitude;
		out.mangitude = 1.0f;
		return out;
	}

	float dotProduct(J_Vector2 a, J_Vector2 b) //Dotproduct of two vectors
	{
		return a.x * b.x + a.y * b.y;
	}

	float dotProduct(J_Vector2 a, float b) //Dotproduct of a vector and a float
	{
		return a.x * b + a.y * b;
	}

	J_Vector2& operator+(const J_Vector2& other) //Overload for addition
	{
		x = x + other.x;
		y = y + other.y;

		return *this;
	}
	J_Vector2& operator-(const J_Vector2& other) //Overload for subtraction
	{
		x = x - other.x;
		y = y - other.y;

		return *this;
	}

private:
};

class J_ForcePlugin
{
public:
	J_ForcePlugin();
	~J_ForcePlugin();

	J_Vector2 Vec2Import(float x, float y, float mangitude)
	{
		return J_Vector2(x, y, mangitude);
	}

	J_Vector2 Vec2Export(float x, float y, float mangitude)
	{

	}

	float test() 
	{
		J_Vector2 test1;
		test1.x = 1;
		test1.y = 2;

		J_Vector2 test2;
		test2.x = 1;
		test2.y = 2;

		return test1.dotProduct(test1, test2);
	};
private:

};


#endif // !J_ForcePlugin_H

