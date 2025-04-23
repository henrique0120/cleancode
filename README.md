# Sistema de Gerenciamento de Biblioteca - README
## Parte 1: Violações Identificadas no Código Original

Antes de realizar a refatoração, várias violações dos princípios SOLID e boas práticas de Clean Code foram identificadas no código original. Abaixo está a descrição detalhada dessas violações:

### 1. Violação do Princípio da Responsabilidade Única (SRP)
**Local**: Classe GerenciadorBiblioteca

**Descrição**: A classe é responsável por gerenciar livros, usuários, empréstimos e notificações (e-mails e SMS). Essas responsabilidades distintas deveriam estar separadas em diferentes classes.

**Problema**: Uma classe que realiza múltiplas tarefas torna-se difícil de manter, testar e estender. Alterações em uma funcionalidade podem impactar outras partes do sistema.

-------------------------------

### 2. Violação do Princípio da Inversão de Dependência (DIP)
**Local:** Métodos EnviarEmail e EnviarSMS na classe GerenciadorBiblioteca

**Descrição:** A classe depende diretamente de implementações específicas para envio de e-mails e SMS.

**Problema:** Isso dificulta a substituição de serviços de notificação ou a reutilização de funcionalidades com outra implementação.

-----------------------

### 3. Violação do Princípio da Substituição de Liskov (LSP)
**Local:** Manipulação de IDs e ISBNs nos métodos RealizarEmprestimo e RealizarDevolucao

**Descrição:** Mistura-se tipos diferentes (ex: int para IDs e string para ISBNs).

**Problema:** Isso cria inconsistências e pode causar falhas se implementações forem alteradas ou substituídas.

------------------------

### 4. Violação das boas práticas de Clean Code (Funções longas e com múltiplas responsabilidades)
**Local:** Métodos RealizarEmprestimo e RealizarDevolucao

**Descrição:** Esses métodos possuem múltiplas responsabilidades, como verificar disponibilidade de livros, gerenciar estados de empréstimos, enviar notificações, calcular multas, entre outros.

**Problema:** Funções longas tornam o código difícil de entender, testar e manter. Métodos devem ser pequenos e focados em uma única tarefa.

----------------------

### 5. Violação do Princípio Aberto/Fechado (OCP)
**Local:** Métodos de notificação dentro da classe GerenciadorBiblioteca

**Descrição:** O envio de notificações (e-mails e SMS) está diretamente implementado na classe, dificultando sua extensão para novos métodos de notificação (ex: notificações push).

**Problema:** Para adicionar novas formas de notificação, seria necessário modificar o código existente, em vez de estendê-lo por meio de novas implementações.

---------------------

## Parte 2: Solução Proposta
A refatoração corrigiu essas violações por meio de:

Separação de responsabilidades em diferentes classes (GerenciadorLivros, GerenciadorUsuarios, GerenciadorEmprestimos, etc.).

Introdução de interfaces para envio de notificações (INotificador) e suas implementações (NotificadorEmail e NotificadorSMS).

Organização do código em funções pequenas, focadas e legíveis.

Uso de classes coesas, cada uma focada em uma única responsabilidade.
