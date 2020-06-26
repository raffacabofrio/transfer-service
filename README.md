# transfer-service

Microserviço cuja responsabilidade é ter uma operação consistente de transferência bancária. É um projeto simples para mostrar conceitos importantes no desenvolvimento de software.
- Simplicidade.
- Robustez.
- Arquitetura DDD.
- Segurança.
- Testes unitários.
- Histórico do git em sequência lógica e educativa.

# Como rodar o projeto?

Edite o arquivo "appsettings.json" e coloque a string de conexão para o seu banco de dados sql server vazio. Ao rodar pela primeira vez, todas as tabelas serão criadas junto de uma massa de teste para vc poder experimentar os endpoints.

Criei uma coleção no postman: https://www.getpostman.com/collections/696823796014e126f186

### Usando a coleção acima sugiro o seguinte teste inicial:

1. Faça o login com o usuário 1.
2. Verifique o Saldo e o extrato do mesmo.
3. Faça uma transferência para o usuário 2.
4. Verifique novamente seu saldo e extrato. Devem estar atualizados com a operação acima.

# Modelo desse projeto

Desde 2018 eu tenho contribuído no projeto open source sharebook. Um app para doação de livros. Usei esse projeto como base. Removi tudo que era relacionado a livro. E criei novas entidades e endpoints para atender esse cenário de transferência bancária.

<img src="docs/design.jpg" width="500"/>

# Banco de dados. Por que escolhi SQL SERVER?

Escolhi o SQL server por ser muito robusto e disponível. Na minha experiência na Coca-Cola USA um banco Sql Server bem configurado aguentava 20 milhões de registros por dia. Atende bem esse cenário aqui. Além de que eu tenho mais facilidade em implementar atomicidade de transação, que é essencial nesse cenário.
