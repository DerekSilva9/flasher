# Flasher

# CS2 External Controller | C# .NET

![Versão](https://img.shields.io/badge/Vers%C3%A3o-1.0.0-blue)
![Linguagem](https://img.shields.io/badge/Linguagem-C%23-green)
![Plataforma](https://img.shields.io/badge/Plataforma-Windows-lightgrey)

Um software externo desenvolvido para fins educacionais, focado em manipulação de memória (Memory Hacking) e automação de processos para o jogo Counter-Strike 2. O projeto apresenta uma interface gráfica (GUI) moderna e multithreaded.

<img width="1071" height="669" alt="image" src="https://github.com/user-attachments/assets/092e2004-ef8b-4b67-931f-3e188fb2089b" />


---

## 🚀 Funcionalidades Principais

O sistema utiliza threads separadas para garantir que a lógica de leitura e escrita de memória não sofra latência da interface do usuário.

* **Recoil Control System (RCS):** Compensação automática do "punch" da arma, calculando a diferença entre os ângulos de visão e o recuo atual.
* **NoFlash:** Monitoramento do estado visual do jogador para anular o efeito de cegueira de granadas de luz.
* **Triggerbot Avançado:** Disparo automático ao detectar inimigos no retículo de mira, com ajustes dinâmicos:
    * **Precision Delay:** Tempo de resposta em milissegundos antes do disparo.
    * **Hold Time:** Tempo em que o gatilho permanece pressionado (essencial para registro de hits).
    * **Cadence:** Intervalo entre disparos consecutivos para controle de spray.

## 🛠️ Detalhes Técnicos

### Arquitetura de Memória
A classe `Memory.cs` encapsula a API nativa do Windows (`kernel32.dll`), permitindo operações seguras de:
- **RPM (ReadProcessMemory):** Leitura de ponteiros e estruturas complexas.
- **WPM (WriteProcessMemory):** Escrita de valores primitivos e vetores (`Vector2`).



### Interface Gráfica (GUI)
Desenvolvida em WinForms com design customizado (Dark Mode), permitindo:
- **Hotkeys:** Ativação/Desativação rápida via F1, F2 e F3.
- **Presets:** Configurações pré-definidas para diferentes categorias de armas (Pistols, Rifles, AWP).

---

## 💻 Como Compilar

1. Certifique-se de ter o **Visual Studio 2022** instalado com suporte a .NET Desktop.
2. Adicione a referência ao namespace `System.Numerics` (para cálculos de vetores).
3. Altere a configuração de build para **Release | x64**.
4. Execute o executável gerado como **Administrador** (necessário para obter privilégios de acesso ao processo `cs2.exe`).

## ⚠️ Disclaimer Técnico

Este repositório foi criado exclusivamente para demonstrar conceitos de:
1. Interoperabilidade entre C# e C++ (P/Invoke).
2. Gerenciamento de processos e threads em Windows.
3. Engenharia reversa de estruturas de dados em tempo de execução.

**O uso deste software em servidores oficiais protegidos pelo Valve Anti-Cheat (VAC) resultará em banimento permanente. Use apenas em ambientes de teste controlados com `-insecure`.**

---

### Autor
Desenvolvido por **[Derek Silva]**. Se este projeto foi útil para o seu aprendizado, considere dar uma ⭐ no repositório!
