# Polimento do hall e interfaces — 22/10/2026

Aplicado à cena existente Teatro_Centro_SESI, preservando palco, plateia, fachada e controlador. Backup anterior em Scenes/Teatro/Antes_Polimento.unity.

## Organização espacial

O visitante entra olhando para +Z; o lado direito é +X. O painel 01 foi transferido da posição solta à esquerda para a parede de fundo à direita, antes do foyer. Painéis 02 e 03 permanecem no foyer direito, com o mesmo eixo de altura e espaçamento regular. Os quatro pontos de interação existentes foram reutilizados; não há novos Canvas ou controladores.

Painéis têm 5–10 mm de afastamento da parede. A placa BILHETERIA foi corrigida no objeto existente, com tipografia centralizada, margens e dois montantes ligados ao balcão. O cartaz foi fixado à parede, e as orientações de auditório/foyer receberam placas de suporte. Monitor e teclado têm apoio no tampo. Rodapés, piso acetinado e luzes de leitura complementam o acabamento. Os detalhes do hall continuam sendo uma interpretação, pois não existe levantamento completo desse interior nas referências disponíveis.

## Interfaces

Acesso sem ingresso agora abre uma janela centralizada: título, instrução, ícone de ingresso, mensagem principal e botões. IR PARA A BILHETERIA fecha o aviso e ativa direção relativa e distância até o balcão; não teletransporta o visitante. A indicação termina quando o ingresso é emitido. Fechar e Esc devolvem os controles. A porta continua bloqueada sem ingresso.

A bilheteria utiliza o mesmo estilo, com cartão centralizado e botões simétricos. A escala da interface é uniforme, preservando proporções e área segura. Nenhum sistema de pagamento ou persistência de ingresso foi acrescentado.

## Manutenção

Menu Teatro SESI > Museu > Polir hall e sinalização reaplica os ajustes aos objetos existentes. Apenas a sub-hierarquia Acabamento_Hall (suportes e acabamentos próprios) é recomposta. Após executar geradores antigos de arquitetura, reaplique esse polimento. Não execute esses geradores para simplesmente abrir ou jogar a cena.

Relatórios: polimento-espacial.txt (afastamentos), museu-runtime.txt (percurso, colisões e ingresso). Capturas Polimento_*.png e Museu_UI_*.png em Lighting. O teste em Play usa o CharacterController existente e métodos reais de interação. Cliques e teclado são conferidos separadamente na Game View.

A inspeção do palco isolou os artefatos: a comparação com material liso e sem sombras manteve os pontos, confirmando a origem nas frestas de 5 mm entre tábuas vistas a distância. As tábuas existentes foram ajustadas ao passo exato de 207 × 1460 mm, sem remoção nem sobreposição. A captura Polimento_Palco.png confirma o piso contínuo. As folhas do auditório foram ajustadas ao vão para reduzir a fresta central, mantendo pivôs e animação.
A galeria esquerda foi reduzida de quatro quadros com fotos repetidas para duas fotografias distintas do usuário. As duas molduras preservadas foram redimensionadas em 4:3, alinhadas e mantidas junto à parede; apenas cópias repetidas foram removidas.

Validação final: PASS em 45,8 s na cena salva, incluindo porta sem ingresso, aviso modal, orientação, cancelamento, emissão única, quatro exposições, animação das folhas, colisão de parede e acesso ao palco. Cliques de orientação/fechar e retirada/conclusão do ingresso também foram confirmados manualmente na Game View Full HD.
