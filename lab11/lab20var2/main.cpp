#include <SFML/Graphics.hpp>
#include <GL/glew.h>
#include <iostream>

int currentFigure = 0; // 0 -  треугольник, 1 - четырёхугольник, 2 - веер, 3 - пятиугольник
int currentColorMode = 0; // 0 - uniform цвет, 1 - градиент

// ID шейдерной программы
GLuint Program;
// ID атрибута
GLint Attrib_vertex;
GLint Attrib_color;
// ID Vertex Buffer Object
GLuint VBO;
GLint uniformR, uniformG, uniformB;

struct Vertex {
	GLfloat x, y;
	GLfloat r, g, b;
};

// Вершинный шейдер
// 0.0 - пустой Z, нет объема; 1.0 - альфа канал полный
const char* VertexShaderSource = R"(
	#version 330 core
	in vec2 coord;
	in vec3 colorVertex; //Цвет вершины (для градиента)
	out vec3 vertexColor; //Передача цвета в фрагментный шейдер

	void main() {
		gl_Position = vec4(coord, 0.0, 1.0); 
		vertexColor = colorVertex;
	
	}
)";
//uniform int useGradient; // Переключатель между uniform и градиентом
//if (useGradient == 1) {
//	VertexColor = colorVertex; //Градиент
//}
//else {
//	VertexColor = vec3(0.0); // Если градиент не нужен
//}
GLint R = 1;
GLint G = 1;
GLint B = 0;

// Исходный код фрагментного (пикселный) шейдера
// R = 0; G = 1; B = 0; Alpha = 1
const char* FragShaderSource = R"(
	#version 330 core
	in vec3 vertexColor;
	out vec4 color;
	
	uniform int useGradient; // Переключатель между uniform и градиентом
	uniform float R;
	uniform float G;
	uniform float B;

	void main() {
		if(useGradient == 1){
			color = vec4(vertexColor, 1.0); // Градиент
		}else{
			color = vec4(R, G, B, 1);
		}
		
	}
)";

void ShaderLog(unsigned int shader)
{
	int infologLen = 0;
	glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
	if (infologLen > 1)
	{
		int charsWritten = 0;
		std::vector<char> infoLog(infologLen);
		glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog.data());
		std::cout << "InfoLog: " << infoLog.data() << std::endl;
	}
}

// Освобождение буфера
void ReleaseVBO() {
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &VBO);
}

// Освобождение шейдеров
void ReleaseShader() {
	// Передавая ноль, мы отключаем шейдерную программу
	glUseProgram(0);
	
	// Удаляем шейдерную программу
	glDeleteProgram(Program);
}

void Release() {
	// Шейдеры
	ReleaseShader();
	// Вершинный буфер
	ReleaseVBO();
}

void Draw() {
	glUseProgram(Program); // Устанавливаем шейдерную программу текущей
	glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // Подключаем VBO
	
	// сообщаем OpenGL как он должен интерпретировать вершинные данные.
	// 2 - берем по 2
	// GL_FALSE - нормализация (от 0 до 2)
	// 0 шаг (берем каждый, он сам считает)
	// берем с 0 элемента буфера (с начала)
	glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(GL_FLOAT), 0);

	GLint Attrib_color = glGetAttribLocation(Program, "vertexColor");
	std::cout << Attrib_color;

	glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GL_FLOAT), (void *)(sizeof(GL_FLOAT) * 2));
	glBindBuffer(GL_ARRAY_BUFFER, 0); // Отключаем VBO

	// начиная с нулевой вершины берем 3
	//glDrawArrays(GL_TRIANGLES, 0, 3); // Передаем данные на видеокарту (рисуем)

	if (currentFigure == 0) 
	{
		glDrawArrays(GL_QUADS, 0, 4);
	}
	else if(currentFigure == 1) 
	{
		glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
	}
	if (currentFigure == 2)
	{
		glDrawArrays(GL_POLYGON, 0, 5);
	}

	glDisableVertexAttribArray(Attrib_vertex); // Отключаем массив атрибутов
	glDisableVertexAttribArray(Attrib_color);
	glUseProgram(0); // Отключаем шейдерную программу
	//checkOpenGLerror();
}

void InitShader() {
	// Создаем вершинный шейдер (сохраняем дескриптор)
	GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
	
	// Передаем исходный код
	glShaderSource(vShader, 1, &VertexShaderSource, NULL);
	
	// Компилируем шейдер
	glCompileShader(vShader);
	std::cout << "vertex shader \n";
	
	// Функция печати лога шейдера
	ShaderLog(vShader); //Пример функции есть в лабораторной

	// Создаем фрагментный шейдер
	GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
	
	// Передаем исходный код
	glShaderSource(fShader, 1, &FragShaderSource, NULL);
	
	// Компилируем шейдер
	glCompileShader(fShader);
	std::cout << "fragment shader \n";
	
	// Функция печати лога шейдера
	ShaderLog(fShader);

	// Создаем программу и прикрепляем шейдеры к ней
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);
	
	// Линкуем шейдерную программу
	glLinkProgram(Program);
	
	//Локации используются для ссылок на эти переменные при передаче данных.
	uniformR = glGetUniformLocation(Program, "R");
	uniformG = glGetUniformLocation(Program, "G");
	uniformB = glGetUniformLocation(Program, "B");

	std::cout << uniformG;
	if (uniformR == -1 || uniformG == -1 || uniformB == -1) 
	{
		std::cerr << "not init uniform shaders" << std::endl;
	}


	// Проверяем статус сборки
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
	if (!link_ok) {
		std::cout << "error attach shaders \n";
		return;
	}

	// Вытягиваем ID атрибута из собранной программы
	const char* attr_name = "coord"; //имя в шейдере
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1) {
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}
	const char* attr_name2 = "vertexColor";
	Attrib_color = glGetAttribLocation(Program, attr_name2);
	if (Attrib_color == -1) 
	{
		std::cout << "" << attr_name2 << "" << std::endl;
	}
	//checkOpenGLerror();
}

void UpdateUniforms() {
	glUseProgram(Program);

	glUniform1f(uniformR, R);
	glUniform1f(uniformG, G);
	glUniform1f(uniformB, B);

	//Передача текущего режима
	GLint uniformGradient = glGetUniformLocation(Program, "useGradient");
	glUniform1i(uniformGradient, currentColorMode);




	glUseProgram(0);
}

void InitVBO() {
	glGenBuffers(1, &VBO);
	Vertex vertices[6];
	int vertexCount = 0;

	if (currentFigure == 0) 
	{
		Vertex quadro[4] = {
			{ -0.5f, -0.5f, 1.0, 0.0, 0.0},
			{  -0.5f, 0.5f, 0.0, 1.0, 0.0 },
			{  0.5f,  0.5f, 0.0, 0.0, 1.0 },
			{  0.5f, -0.5f, 1.0, 1.0, 0.0  }
		};
		vertexCount = 4;
		std::copy(quadro, quadro + 4, vertices);
		for (int i = 0; i < vertexCount; i++) 
		{
			vertices[i].r = (vertices[i].x + 1.0) / 2.0f;
			vertices[i].g = (vertices[i].y + 1.0) / 2.0f;
			vertices[i].b = 1.0f - vertices[i].r - vertices[i].g;
		}

	}
	else if (currentFigure == 1) 
	{
		Vertex veer[6] = {
			{ 0.0f, 0.0f },
			{ -0.6f, 0.2f },
			{ -0.4f, 0.4f },
			{ 0.0f, 0.5f },
			{ 0.5f, 0.5f },
			{ 0.8f, 0.0f }
		};

		std::copy(veer, veer + 6, vertices);
		vertexCount = 6;

		for (int i = 0; i < vertexCount; i++)
		{
			vertices[i].r = (vertices[i].x + 1.0) / 2.0f;
			vertices[i].g = (vertices[i].y + 1.0) / 2.0f;
			vertices[i].b = 1.0f - vertices[i].r - vertices[i].g;
		}

	}
	else if (currentFigure == 2)
	{
		Vertex penta[5] = {
			{ 0.0f, 1.0f },
			{ 1.0f, 0.3f },
			{ 0.6f, -0.8f },
			{ -0.6f, -0.8f },
			{ -1.0f, 0.3f }

		};

		std::copy(penta, penta + 5, vertices);
		vertexCount = 5;

		for (int i = 0; i < vertexCount; i++)
		{
			vertices[i].r = (vertices[i].x + 1.0) / 2.0f;
			vertices[i].g = (vertices[i].y + 1.0) / 2.0f;
			vertices[i].b = 1.0f - vertices[i].r - vertices[i].g;
		}


	}

	//Vertex*  BufferFigure = fig.triangle;

	// Передаем вершины в буфер
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertexCount, vertices, GL_STATIC_DRAW);
	//checkOpenGLerror(); //Пример функции есть в лабораторной
	// Проверка ошибок OpenGL, если есть, то вывод в консоль тип ошибки
	glVertexAttribPointer(Attrib_vertex, 5, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)0);
	glEnableVertexAttribArray(Attrib_vertex);

	//GLuint Attrib_color = glGetAttribLocation(Program, "colorVertex");
	//glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(2 * sizeof(GLfloat)));
	//glEnableVertexAttribArray(Attrib_color);
}

void Init() {
	// Шейдеры
	InitShader();
	// Вершинный буфер, VBO (Vertex Buffer Object) - информация о вершинах
	InitVBO();
}

int main() {
	sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
	window.setVerticalSyncEnabled(true);
	window.setActive(true);

	glewInit();
	// Шейдеры и вершинный буфер
	Init();
	
	while (window.isOpen()) 
	{
		sf::Event event;

		while (window.pollEvent(event)) 
		{
			if (event.type == sf::Event::Closed) { window.close(); }
			else if (event.type == sf::Event::Resized) { glViewport(0, 0, event.size.width, event.size.height); }
			else if (event.type == sf::Event::MouseButtonPressed) 
			{
				currentFigure = (currentFigure + 1 ) % 3; // переключаем фигуру
				currentColorMode = (currentColorMode + 1) % 2;
				std::cout << "colormode: " << currentColorMode << "\n";
				InitVBO();
			}
		}
		
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		UpdateUniforms();
		Draw();
		window.display();
	}
	
	// Шейдеры и вершинный буфер
	Release();
	return 0;
}